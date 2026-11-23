using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Repositories;
using Application.Rooms.Exceptions;
using Domain.Categories;
using Domain.Rooms;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Rooms.Commands;

public record UpdateRoomCommand : IRequest<Either<RoomException, Room>>
{
    public required Guid RoomId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required IReadOnlyCollection<Guid> CategoryIds { get; init; }
    public int? Floor { get; init; }
    public int? Capacity { get; init; }
    public string? ViewType { get; init; }
}

public class UpdateRoomCommandHandler(
    IRoomRepository roomRepository,
    ICategoryRepository categoryRepository,
    IRoomCategoryRepository roomCategoryRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateRoomCommand, Either<RoomException, Room>>
{
    public async Task<Either<RoomException, Room>> Handle(
        UpdateRoomCommand request,
        CancellationToken cancellationToken)
    {
        var roomId = new RoomId(request.RoomId);

        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var room = await roomRepository.GetByIdAsync(roomId, cancellationToken);

            var result = await room.MatchAsync(
                r => CheckDuplicatesAsync(r.Id, request.Title, cancellationToken)
                    .BindAsync(_ => CheckCategoriesAsync(r.Id, request.CategoryIds, cancellationToken))
                    .BindAsync(categoryIds => UpdateEntityAsync(r, request, categoryIds, cancellationToken)),
                () => (Either<RoomException, Room>)new RoomNotFoundException(roomId));

            await result.MatchAsync(
                async _ =>
                {
                    await transaction.CommitAsync(cancellationToken);
                    return Unit.Default;
                },
                async _ =>
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Unit.Default;
                });

            return result;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new RoomUnhandledException(roomId, exception);
        }
    }

    private async Task<Either<RoomException, Unit>> CheckDuplicatesAsync(
        RoomId currentRoomId,
        string title,
        CancellationToken cancellationToken)
    {
        var existing = await roomRepository.GetByTitleAsync(title, cancellationToken);

        return existing.Match<Either<RoomException, Unit>>(
            r => r.Id == currentRoomId ? Unit.Default : new RoomAlreadyExistsException(r.Id),
            () => Unit.Default);
    }

    private async Task<Either<RoomException, IReadOnlyCollection<CategoryId>>> CheckCategoriesAsync(
        RoomId roomId,
        IReadOnlyCollection<Guid> rawCategoryIds,
        CancellationToken cancellationToken)
    {
        var requestedIds = rawCategoryIds.Distinct().Select(id => new CategoryId(id)).ToList();
        var existingCategories = await categoryRepository.GetByIdsAsync(requestedIds, cancellationToken);
        var existingIds = existingCategories.Select(c => c.Id).ToHashSet();

        var missingIds = requestedIds.Where(id => !existingIds.Contains(id)).ToList();

        return missingIds.Any()
            ? new RoomCategoriesNotFoundException(roomId, missingIds)
            : requestedIds;
    }

    private async Task<Either<RoomException, Room>> UpdateEntityAsync(
        Room room,
        UpdateRoomCommand request,
        IReadOnlyCollection<CategoryId> requestedCategoryIds,
        CancellationToken cancellationToken)
    {
        var attributes = new RoomAttributes(request.Floor, request.Capacity, request.ViewType);
        room.UpdateDetails(request.Title, request.Description, attributes);

        var requestedSet = requestedCategoryIds.ToHashSet();
        var existingSet = room.Categories.Select(rc => rc.CategoryId).ToHashSet();

        var toRemove = room.Categories
            .Where(rc => !requestedSet.Contains(rc.CategoryId))
            .ToList();

        var toAdd = requestedSet
            .Where(id => !existingSet.Contains(id))
            .Select(id => new RoomCategory(room.Id, id))
            .ToList();

        if (toRemove.Any())
        {
            await roomCategoryRepository.RemoveRangeAsync(toRemove, cancellationToken);
        }

        if (toAdd.Any())
        {
            await roomCategoryRepository.AddRangeAsync(toAdd, cancellationToken);
        }

        await roomRepository.UpdateAsync(room, cancellationToken);

        var refreshed = await roomRepository.GetByIdAsync(room.Id, cancellationToken);
        return refreshed.Match<Either<RoomException, Room>>(
            r => r,
            () => new RoomNotFoundException(room.Id));
    }
}
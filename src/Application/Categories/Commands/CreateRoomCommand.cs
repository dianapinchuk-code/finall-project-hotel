using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Rooms.Exceptions;
using Domain.Categories;
using Domain.Rooms;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace Application.Rooms.Commands;

public record CreateRoomCommand : IRequest<Either<RoomException, Room>>
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required IReadOnlyCollection<Guid> CategoryIds { get; init; }
    public int? Floor { get; init; }
    public int? Capacity { get; init; }
    public string? ViewType { get; init; }
}

public class CreateRoomCommandHandler(
    IRoomRepository roomRepository,
    ICategoryRepository categoryRepository,
    INotificationService notificationService)
    : IRequestHandler<CreateRoomCommand, Either<RoomException, Room>>
{
    public async Task<Either<RoomException, Room>> Handle(
        CreateRoomCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await roomRepository.GetByTitleAsync(request.Title, cancellationToken);

        return await existing.MatchAsync(
            r => (Either<RoomException, Room>)new RoomAlreadyExistsException(r.Id),
            () => CheckCategoriesAsync(request.CategoryIds, cancellationToken)
                .BindAsync(categories => CreateEntityAsync(request, categories, cancellationToken)));
    }

    private async Task<Either<RoomException, IReadOnlyCollection<CategoryId>>> CheckCategoriesAsync(
        IReadOnlyCollection<Guid> rawCategoryIds,
        CancellationToken cancellationToken)
    {
        var requestedIds = rawCategoryIds.Distinct().Select(id => new CategoryId(id)).ToList();
        var existingCategories = await categoryRepository.GetByIdsAsync(requestedIds, cancellationToken);
        var existingIds = existingCategories.Select(c => c.Id).ToHashSet();

        var missingIds = requestedIds.Where(id => !existingIds.Contains(id)).ToList();

        return missingIds.Any()
            ? new RoomCategoriesNotFoundException(RoomId.Empty(), missingIds)
            : requestedIds;
    }

    private async Task<Either<RoomException, Room>> CreateEntityAsync(
        CreateRoomCommand request,
        IReadOnlyCollection<CategoryId> categoryIds,
        CancellationToken cancellationToken)
    {
        try
        {
            var attributes = new RoomAttributes(request.Floor, request.Capacity, request.ViewType);
            var room = Room.New(RoomId.New(), request.Title, request.Description, attributes);

            foreach (var categoryId in categoryIds)
            {
                room.AddCategory(new RoomCategory(room.Id, categoryId));
            }

            var createdRoom = await roomRepository.AddAsync(room, cancellationToken);
            await notificationService.SendRoomCreatedNotificationAsync(createdRoom, cancellationToken);

            return createdRoom;
        }
        catch (Exception exception)
        {
            return new RoomUnhandledException(RoomId.Empty(), exception);
        }
    }
}
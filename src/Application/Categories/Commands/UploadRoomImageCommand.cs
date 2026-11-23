using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Rooms.Exceptions;
using Domain.Rooms;
using LanguageExt;
using MediatR;

namespace Application.Rooms.Commands;

public record UploadRoomImageCommand : IRequest<Either<RoomException, RoomImage>>
{
    public required Guid RoomId { get; init; }
    public required string OriginalName { get; init; }
    public required Stream Content { get; init; }
}

public class UploadRoomImageCommandHandler(
    IRoomRepository roomRepository,
    IFileStorage fileStorage)
    : IRequestHandler<UploadRoomImageCommand, Either<RoomException, RoomImage>>
{
    public async Task<Either<RoomException, RoomImage>> Handle(
        UploadRoomImageCommand request,
        CancellationToken cancellationToken)
    {
        var roomId = new RoomId(request.RoomId);
        var room = await roomRepository.GetByIdAsync(roomId, cancellationToken);

        return await room.MatchAsync(
            r => SaveImageAsync(r, request, cancellationToken),
            () => (Either<RoomException, RoomImage>)new RoomNotFoundException(roomId));
    }

    private async Task<Either<RoomException, RoomImage>> SaveImageAsync(
        Room room,
        UploadRoomImageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var image = RoomImage.New(RoomImageId.New(), request.OriginalName, room.Id);
            var savedImage = await roomRepository.AddImageAsync(image, cancellationToken);

            var extension = Path.GetExtension(request.OriginalName);
            var relativePath = $"{room.Id}/{savedImage.Id}{extension}";

            await fileStorage.SaveAsync(relativePath, request.Content, cancellationToken);

            return savedImage;
        }
        catch (Exception exception)
        {
            return new RoomUnhandledException(room.Id, exception);
        }
    }
}
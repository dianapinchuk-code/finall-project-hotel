using Application.Common.Interfaces.Repositories;
using Application.Rooms.Exceptions;
using Domain.Rooms;
using LanguageExt;
using MediatR;

namespace Application.Rooms.Commands;

public record DeleteRoomCommand : IRequest<Either<RoomException, Room>>
{
    public required Guid RoomId { get; init; }
}

public class DeleteRoomCommandHandler(IRoomRepository roomRepository)
    : IRequestHandler<DeleteRoomCommand, Either<RoomException, Room>>
{
    public async Task<Either<RoomException, Room>> Handle(
        DeleteRoomCommand request,
        CancellationToken cancellationToken)
    {
        var roomId = new RoomId(request.RoomId);
        var room = await roomRepository.GetByIdAsync(roomId, cancellationToken);

        return await room.MatchAsync(
            async r =>
            {
                try
                {
                    await roomRepository.DeleteAsync(r, cancellationToken);
                    return (Either<RoomException, Room>)r;
                }
                catch (Exception ex)
                {
                    return (Either<RoomException, Room>)new RoomUnhandledException(roomId, ex);
                }
            },
            () => (Either<RoomException, Room>)new RoomNotFoundException(roomId));
    }
}
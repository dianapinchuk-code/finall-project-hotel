using Domain.Rooms;
using LanguageExt;

namespace Application.Common.Interfaces.Queries;

public interface IRoomQueries
{
    Task<IReadOnlyList<Room>> GetAllAsync(CancellationToken cancellationToken);
    Task<Option<Room>> GetByIdAsync(RoomId id, CancellationToken cancellationToken);
}
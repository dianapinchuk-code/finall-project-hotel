using Domain.Rooms;
using LanguageExt;

namespace Application.Common.Interfaces.Repositories;

public interface IRoomRepository
{
    Task<Option<Room>> GetByIdAsync(RoomId id, CancellationToken cancellationToken);
    Task<Option<Room>> GetByTitleAsync(string title, CancellationToken cancellationToken);
    Task<Room> AddAsync(Room room, CancellationToken cancellationToken);
    Task<Room> UpdateAsync(Room room, CancellationToken cancellationToken);
    Task DeleteAsync(Room room, CancellationToken cancellationToken);
    Task<RoomImage> AddImageAsync(RoomImage image, CancellationToken cancellationToken);
}
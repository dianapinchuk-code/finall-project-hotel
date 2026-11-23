using Domain.Rooms;

namespace Application.Common.Interfaces.Repositories;

public interface IRoomCategoryRepository
{
    Task AddRangeAsync(IReadOnlyCollection<RoomCategory> links, CancellationToken cancellationToken);
    Task RemoveRangeAsync(IReadOnlyCollection<RoomCategory> links, CancellationToken cancellationToken);
}
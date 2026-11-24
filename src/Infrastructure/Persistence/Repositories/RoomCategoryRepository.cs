using Application.Common.Interfaces.Repositories;
using Domain.Rooms;

namespace Infrastructure.Persistence.Repositories;

public class RoomCategoryRepository(ApplicationDbContext context) : IRoomCategoryRepository
{
    public async Task AddRangeAsync(IReadOnlyCollection<RoomCategory> links, CancellationToken cancellationToken)
    {
        await context.RoomCategories.AddRangeAsync(links, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveRangeAsync(IReadOnlyCollection<RoomCategory> links, CancellationToken cancellationToken)
    {
        context.RoomCategories.RemoveRange(links);
        await context.SaveChangesAsync(cancellationToken);
    }
}
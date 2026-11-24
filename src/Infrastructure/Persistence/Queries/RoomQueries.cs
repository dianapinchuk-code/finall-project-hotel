using Application.Common.Interfaces.Queries;
using Domain.Rooms;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace Infrastructure.Persistence.Queries;

public class RoomQueries(ApplicationDbContext context) : IRoomQueries
{
    public async Task<IReadOnlyList<Room>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Rooms
            .AsNoTracking()
            .Include(r => r.Categories)
            .ThenInclude(rc => rc.Category)
            .Include(r => r.Images)
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Room>> GetByIdAsync(RoomId id, CancellationToken cancellationToken)
    {
        var entity = await context.Rooms
            .AsNoTracking()
            .Include(r => r.Categories)
            .ThenInclude(rc => rc.Category)
            .Include(r => r.Images)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return Optional(entity);
    }
}
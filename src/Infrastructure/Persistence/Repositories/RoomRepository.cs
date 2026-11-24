using Application.Common.Interfaces.Repositories;
using Domain.Rooms;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace Infrastructure.Persistence.Repositories;

public class RoomRepository(ApplicationDbContext context) : IRoomRepository
{
    public async Task<Option<Room>> GetByIdAsync(RoomId id, CancellationToken cancellationToken)
    {
        var entity = await context.Rooms
            .Include(r => r.Categories)
            .ThenInclude(rc => rc.Category)
            .Include(r => r.Images)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return Optional(entity);
    }

    public async Task<Option<Room>> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        var entity = await context.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Title == title, cancellationToken);

        return Optional(entity);
    }

    public async Task<Room> AddAsync(Room room, CancellationToken cancellationToken)
    {
        await context.Rooms.AddAsync(room, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return room;
    }

    public async Task<Room> UpdateAsync(Room room, CancellationToken cancellationToken)
    {
        if (context.Entry(room).State == EntityState.Detached)
        {
            context.Rooms.Update(room);
        }

        await context.SaveChangesAsync(cancellationToken);
        return room;
    }

    public async Task DeleteAsync(Room room, CancellationToken cancellationToken)
    {
        context.Rooms.Remove(room);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<RoomImage> AddImageAsync(RoomImage image, CancellationToken cancellationToken)
    {
        await context.RoomImages.AddAsync(image, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return image;
    }
}
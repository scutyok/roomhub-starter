using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class RoomRepository(RoomHubDbContext db) : IRoomRepository
{
    public Task<Room?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return db.Rooms.FirstOrDefaultAsync(room => room.Id == id, cancellationToken);
    }

    public async Task AddAsync(Room room, CancellationToken cancellationToken = default)
    {
        await db.Rooms.AddAsync(room, cancellationToken);
    }
}

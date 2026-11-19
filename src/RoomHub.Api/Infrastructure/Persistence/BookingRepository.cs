using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class BookingRepository(RoomHubDbContext db) : IBookingRepository
{
    public Task<Booking?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return db.Bookings.FirstOrDefaultAsync(booking => booking.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Booking>> ListActiveForRoomAsync(
        Guid roomId,
        CancellationToken cancellationToken = default)
    {
        return await db.Bookings
            .Where(booking => booking.RoomId == roomId && booking.Status == BookingStatus.Active)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        await db.Bookings.AddAsync(booking, cancellationToken);
    }
}

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

    public Task<int> CountActiveForParticipantOnUtcDateAsync(
        string bookedBy,
        DateTime utcDate,
        CancellationToken cancellationToken = default)
    {
        var start = utcDate.Date;
        var end = start.AddDays(1);

        return db.Bookings.CountAsync(
            booking =>
                booking.BookedBy == bookedBy
                && booking.Status == BookingStatus.Active
                && booking.StartUtc >= start
                && booking.StartUtc < end,
            cancellationToken);
    }

    public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        await db.Bookings.AddAsync(booking, cancellationToken);
    }
}

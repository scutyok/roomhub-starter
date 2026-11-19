using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Infrastructure.Persistence;

public sealed class RoomHubDbContext(DbContextOptions<RoomHubDbContext> options) : DbContext(options)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(room => room.Id);
            entity.Property(room => room.Name).IsRequired();
            entity.Property(room => room.Capacity).IsRequired();
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(booking => booking.Id);
            entity.Property(booking => booking.BookedBy).IsRequired();
            entity.Property(booking => booking.StartUtc).IsRequired();
            entity.Property(booking => booking.EndUtc).IsRequired();
            entity.Property(booking => booking.Attendees).IsRequired();
            entity.Property(booking => booking.Status).IsRequired();
            entity.HasIndex(booking => new { booking.RoomId, booking.Status, booking.StartUtc, booking.EndUtc });
        });
    }
}

using RoomHub.Api.Shared;

namespace RoomHub.Api.Domain;

public sealed class Booking
{
    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; } = BookingStatus.Active;

    private Booking()
    {
    }

    private Booking(Guid roomId, string bookedBy, DateTime startUtc, DateTime endUtc, int attendees)
    {
        Id = Guid.NewGuid();
        RoomId = roomId;
        BookedBy = bookedBy;
        StartUtc = startUtc;
        EndUtc = endUtc;
        Attendees = attendees;
    }

    public static Result<Booking> Create(Guid roomId, string? bookedBy, DateTime startUtc, DateTime endUtc, int attendees, Room room, DateTime now)
    {
        if (string.IsNullOrWhiteSpace(bookedBy))
            return Result<Booking>.Failure(new Error("InvalidBookedBy", "BookedBy is required.", 400));
        if (endUtc <= startUtc)
            return Result<Booking>.Failure(new Error("InvalidRange", "The end must be after the start.", 400));
        if (attendees <= 0)
            return Result<Booking>.Failure(new Error("InvalidAttendees", "Attendees must be positive.", 400));
        if (attendees > room.Capacity)
            return Result<Booking>.Failure(new Error("AttendeesExceedCapacity", "The room does not have enough seats.", 400));
        if (startUtc < now)
            return Result<Booking>.Failure(new Error("BookingInPast", "A booking cannot start in the past.", 400));
        if (endUtc - startUtc > TimeSpan.FromHours(4))
            return Result<Booking>.Failure(new Error("InvalidDuration", "A booking cannot last longer than 4 hours.", 400));

        return Result<Booking>.Success(new Booking(roomId, bookedBy.Trim(), startUtc, endUtc, attendees));
    }

    public bool Overlaps(Booking other)
    {
        return Status == BookingStatus.Active
            && other.Status == BookingStatus.Active
            && StartUtc < other.EndUtc
            && other.StartUtc < EndUtc;
    }

    public Result<bool> Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            return Result<bool>.Failure(new Error("BookingAlreadyCancelled", "The booking is already cancelled.", 409));

        Status = BookingStatus.Cancelled;
        return Result<bool>.Success(true);
    }
}

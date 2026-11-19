using RoomHub.Api.Application;
using RoomHub.Api.Domain;
using RoomHub.Api.Shared;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateRoomBookingRequest(
    string? BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees);

public sealed record CreateBookingCommand(
    Guid RoomId,
    string? BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees);

public sealed record BookingCreatedResult(Guid Id, Guid RoomId, string Status);

public sealed class CreateBookingHandler(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) :
    ICommandHandler<CreateBookingCommand, BookingCreatedResult>
{
    public async Task<Result<BookingCreatedResult>> HandleAsync(
        CreateBookingCommand command,
        CancellationToken cancellationToken = default)
    {
        var room = await rooms.GetAsync(command.RoomId, cancellationToken);
        if (room is null)
        {
            return Result<BookingCreatedResult>.Failure(
                new Error("RoomNotFound", "The room does not exist.", 404));
        }

        var creation = Booking.Create(
            command.RoomId,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            room,
            timeProvider.GetUtcNow().UtcDateTime);

        if (!creation.IsSuccess)
            return Result<BookingCreatedResult>.Failure(creation.Error!);

        var booking = creation.Value!;
        var activeBookings = await bookings.ListActiveForRoomAsync(command.RoomId, cancellationToken);
        if (activeBookings.Any(existing => existing.Overlaps(booking)))
        {
            return Result<BookingCreatedResult>.Failure(
                new Error("RoomOccupied", "The room is occupied in this interval.", 409));
        }

        await bookings.AddAsync(booking, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<BookingCreatedResult>.Success(
            new BookingCreatedResult(booking.Id, booking.RoomId, booking.Status.ToString()));
    }
}

using RoomHub.Api.Application;
using RoomHub.Api.Domain;
using RoomHub.Api.Shared;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed record GetBookingQuery(Guid Id);
public sealed record BookingResult(
    Guid Id,
    Guid RoomId,
    string BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees,
    string Status);

public sealed class GetBookingHandler(IBookingRepository bookings) :
    ICommandHandler<GetBookingQuery, BookingResult>
{
    public async Task<Result<BookingResult>> HandleAsync(
        GetBookingQuery query,
        CancellationToken cancellationToken = default)
    {
        var booking = await bookings.GetAsync(query.Id, cancellationToken);
        if (booking is null)
        {
            return Result<BookingResult>.Failure(
                new Error("BookingNotFound", "The booking does not exist.", 404));
        }

        return Result<BookingResult>.Success(new BookingResult(
            booking.Id,
            booking.RoomId,
            booking.BookedBy,
            booking.StartUtc,
            booking.EndUtc,
            booking.Attendees,
            booking.Status.ToString()));
    }
}

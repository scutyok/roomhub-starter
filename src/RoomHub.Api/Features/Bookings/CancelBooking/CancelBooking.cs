using RoomHub.Api.Application;
using RoomHub.Api.Domain;
using RoomHub.Api.Shared;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed record CancelBookingCommand(Guid Id);

public sealed class CancelBookingHandler(
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) :
    ICommandHandler<CancelBookingCommand, Unit>
{
    public async Task<Result<Unit>> HandleAsync(
        CancelBookingCommand command,
        CancellationToken cancellationToken = default)
    {
        var booking = await bookings.GetAsync(command.Id, cancellationToken);
        if (booking is null)
        {
            return Result<Unit>.Failure(
                new Error("BookingNotFound", "The booking does not exist.", 404));
        }

        var cancellation = booking.Cancel(timeProvider.GetUtcNow().UtcDateTime);
        if (!cancellation.IsSuccess)
            return Result<Unit>.Failure(cancellation.Error!);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Unit>.Success(Unit.Value);
    }
}

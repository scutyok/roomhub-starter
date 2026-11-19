using RoomHub.Api.Application;
using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.GetBooking;
using RoomHub.Api.Shared;

namespace RoomHub.Api.Features.Bookings;

public static class BookingEndpoints
{
    public static IEndpointRouteBuilder MapBookingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/bookings").WithTags("Bookings");

        group.MapGet("/{id:guid}", async (
            Guid id,
            ICommandHandler<GetBookingQuery, BookingResult> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetBookingQuery(id), cancellationToken);
            return MapResult(result, booking =>
                Results.Ok(new
                {
                    id = booking.Id,
                    roomId = booking.RoomId,
                    bookedBy = booking.BookedBy,
                    startUtc = booking.StartUtc,
                    endUtc = booking.EndUtc,
                    attendees = booking.Attendees,
                    status = booking.Status
                }));
        });

        group.MapPost("/{id:guid}/cancel", async (
            Guid id,
            ICommandHandler<CancelBookingCommand, Unit> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new CancelBookingCommand(id), cancellationToken);
            if (result.IsSuccess)
                return Results.NoContent();

            var error = result.Error!;
            return Results.Problem(
                detail: error.Message,
                statusCode: error.StatusCode,
                title: error.Code);
        });

        return app;
    }

    private static IResult MapResult<T>(Result<T> result, Func<T, IResult> success)
    {
        if (result.IsSuccess)
            return success(result.Value!);

        var error = result.Error!;
        return Results.Problem(
            detail: error.Message,
            statusCode: error.StatusCode,
            title: error.Code);
    }
}

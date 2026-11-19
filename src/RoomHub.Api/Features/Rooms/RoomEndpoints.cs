using RoomHub.Api.Application;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoom;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Shared;

namespace RoomHub.Api.Features.Rooms;

public record CreateRoomRequest(string? Name, int Capacity);
public record RoomResponse(Guid Id, string Name, int Capacity);
public record BookingCreatedResponse(Guid Id, Guid RoomId, string Status);

public static class RoomEndpoints
{
    public static IEndpointRouteBuilder MapRoomEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/rooms").WithTags("Rooms");

        group.MapPost("/", async (
            CreateRoomRequest request,
            ICommandHandler<CreateRoomCommand, RoomResult> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(
                new CreateRoomCommand(request.Name, request.Capacity),
                cancellationToken);

            return MapResult(result, room =>
                Results.Created(
                    $"/api/rooms/{room.Id}",
                    new RoomResponse(room.Id, room.Name, room.Capacity)));
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            ICommandHandler<GetRoomQuery, GetRoomResult> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetRoomQuery(id), cancellationToken);
            return MapResult(result, room =>
                Results.Ok(new RoomResponse(room.Id, room.Name, room.Capacity)));
        });

        group.MapPost("/{roomId:guid}/bookings", async (
            Guid roomId,
            CreateRoomBookingRequest request,
            ICommandHandler<CreateBookingCommand, BookingCreatedResult> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(
                new CreateBookingCommand(
                    roomId,
                    request.BookedBy,
                    request.StartUtc,
                    request.EndUtc,
                    request.Attendees),
                cancellationToken);

            return MapResult(result, booking =>
                Results.Created(
                    $"/api/bookings/{booking.Id}",
                    new BookingCreatedResponse(
                        booking.Id,
                        booking.RoomId,
                        booking.Status)));
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

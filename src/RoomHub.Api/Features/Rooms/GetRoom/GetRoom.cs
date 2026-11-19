using RoomHub.Api.Application;
using RoomHub.Api.Domain;
using RoomHub.Api.Shared;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed record GetRoomQuery(Guid Id);
public sealed record GetRoomResult(Guid Id, string Name, int Capacity);

public sealed class GetRoomHandler(IRoomRepository rooms) :
    ICommandHandler<GetRoomQuery, GetRoomResult>
{
    public async Task<Result<GetRoomResult>> HandleAsync(
        GetRoomQuery query,
        CancellationToken cancellationToken = default)
    {
        var room = await rooms.GetAsync(query.Id, cancellationToken);
        if (room is null)
        {
            return Result<GetRoomResult>.Failure(
                new Error("RoomNotFound", "The room does not exist.", 404));
        }

        return Result<GetRoomResult>.Success(new GetRoomResult(room.Id, room.Name, room.Capacity));
    }
}

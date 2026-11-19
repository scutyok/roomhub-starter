using RoomHub.Api.Application;
using RoomHub.Api.Domain;
using RoomHub.Api.Shared;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomCommand(string? Name, int Capacity);
public sealed record RoomResult(Guid Id, string Name, int Capacity);

public sealed class CreateRoomHandler(
    IRoomRepository rooms,
    IUnitOfWork unitOfWork) :
    ICommandHandler<CreateRoomCommand, RoomResult>
{
    public async Task<Result<RoomResult>> HandleAsync(
        CreateRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        var creation = Room.Create(command.Name, command.Capacity);
        if (!creation.IsSuccess)
            return Result<RoomResult>.Failure(creation.Error!);

        var room = creation.Value!;
        await rooms.AddAsync(room, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<RoomResult>.Success(new RoomResult(room.Id, room.Name, room.Capacity));
    }
}

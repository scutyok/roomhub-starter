using RoomHub.Api.Shared;

namespace RoomHub.Api.Domain;

public sealed class Room
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Capacity { get; private set; }

    private Room()
    {
    }

    private Room(Guid id, string name, int capacity)
    {
        Id = id;
        Name = name;
        Capacity = capacity;
    }

    public static Result<Room> Create(string? name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Room>.Failure(new Error("InvalidName", "Room name is required.", 400));
        if (capacity <= 0)
            return Result<Room>.Failure(new Error("InvalidCapacity", "Capacity must be positive.", 400));

        return Result<Room>.Success(new Room(Guid.NewGuid(), name.Trim(), capacity));
    }
}

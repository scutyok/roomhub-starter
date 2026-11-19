namespace RoomHub.Api.Shared;

public sealed record Error(string Code, string Message, int StatusCode);

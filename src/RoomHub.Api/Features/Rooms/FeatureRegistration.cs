using RoomHub.Api.Application;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoom;

namespace RoomHub.Api.Features.Rooms;

public static class FeatureRegistration
{
    public static IServiceCollection AddRoomFeatures(this IServiceCollection services)
    {
        services.AddScoped<
            ICommandHandler<CreateRoomCommand, RoomResult>,
            CreateRoomHandler>();
        services.AddScoped<
            ICommandHandler<GetRoomQuery, GetRoomResult>,
            GetRoomHandler>();
        return services;
    }
}

using RoomHub.Api.Application;
using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBooking;
using RoomHub.Api.Shared;

namespace RoomHub.Api.Features.Bookings;

public static class FeatureRegistration
{
    public static IServiceCollection AddBookingFeatures(this IServiceCollection services)
    {
        services.AddScoped<
            ICommandHandler<CreateBookingCommand, BookingCreatedResult>,
            CreateBookingHandler>();
        services.AddScoped<
            ICommandHandler<GetBookingQuery, BookingResult>,
            GetBookingHandler>();
        services.AddScoped<
            ICommandHandler<CancelBookingCommand, Unit>,
            CancelBookingHandler>();
        return services;
    }
}

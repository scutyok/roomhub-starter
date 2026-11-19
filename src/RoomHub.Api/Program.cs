using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain;
using RoomHub.Api.Features.Bookings;
using RoomHub.Api.Features.Rooms;
using RoomHub.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddDbContext<RoomHubDbContext>(options =>
    options.UseSqlite("Data Source=roomhub.db"));

builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddRoomFeatures();
builder.Services.AddBookingFeatures();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RoomHubDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/", () => "RoomHub API is running.");
app.MapRoomEndpoints();
app.MapBookingEndpoints();

app.Run();

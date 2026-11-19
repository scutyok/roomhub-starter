using RoomHub.Api.Shared;

namespace RoomHub.Api.Application;

public interface ICommandHandler<in TCommand, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

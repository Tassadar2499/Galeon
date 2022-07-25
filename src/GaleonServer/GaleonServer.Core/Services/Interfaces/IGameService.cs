namespace GaleonServer.Core.Services.Interfaces;

public interface IGameService
{
    Task<TResult> Handle<TRequest, TResult>(TRequest request, CancellationToken cancellationToken);

    IAsyncEnumerable<TResult> HandleEnumerable<TRequest, TResult>(TRequest request, CancellationToken cancellationToken);
}
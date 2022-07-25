namespace GaleonServer.Core.Services.Interfaces;

public interface IAuthorizationService
{
    public Task<TResult> Handle<TRequest, TResult>(TRequest request, CancellationToken cancellationToken);
}
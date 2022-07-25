namespace GaleonServer.Core.Services.Interfaces;

public interface IUserService
{
    Task<TResult> Handle<TRequest, TResult>(TRequest request, CancellationToken cancellationToken);
}
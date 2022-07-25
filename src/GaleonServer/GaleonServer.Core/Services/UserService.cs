using GaleonServer.Core.Services.Interfaces;
using GaleonServer.Interfaces.Gateways;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Responses;

namespace GaleonServer.Core.Services;

public class UserService : IUserService
{
    private readonly Lazy<IUserGateway> _userGateway;

    public UserService(Lazy<IUserGateway> userGateway)
    {
        _userGateway = userGateway;
    }

    public async Task<TResult> Handle<TRequest, TResult>(TRequest request, CancellationToken cancellationToken)
    {
        object response = request switch
        {
            DeleteUserCommand deleteUserCommand => await HandleDeleteUser(deleteUserCommand, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(request), request, null)
        };

        return (TResult) response;
    }
    
    private async Task<SimpleResponse> HandleDeleteUser(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userGateway.Value.DeleteUserById(request.Id, cancellationToken);
        
        return SimpleResponse.CreateSucceed();
    }
}
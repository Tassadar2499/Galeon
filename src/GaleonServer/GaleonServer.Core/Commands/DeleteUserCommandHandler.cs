using GaleonServer.Interfaces.Gateways;
using GaleonServer.Models.Commands;
using MediatR;

namespace GaleonServer.Core.Commands;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserGateway _userGateway;

    public DeleteUserCommandHandler(IUserGateway userGateway)
    {
        _userGateway = userGateway;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userGateway.DeleteUserById(request.Id, cancellationToken);
        
        return new();
    }
}
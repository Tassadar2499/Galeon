using GaleonServer.Core.Models;
using GaleonServer.Interfaces.Gateways;
using GaleonServer.Models.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GaleonServer.Core.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
{
    private readonly IEmailGateway _emailGateway;
    private readonly UserManager<User> _userManager;

    public RegisterCommandHandler(IEmailGateway emailGateway, UserManager<User> userManager)
    {
        _emailGateway = emailGateway;
        _userManager = userManager;
    }

    public Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
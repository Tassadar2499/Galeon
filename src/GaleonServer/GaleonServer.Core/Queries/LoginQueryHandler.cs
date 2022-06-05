using System.Net;
using GaleonServer.Core.Exceptions;
using GaleonServer.Core.Gateways;
using GaleonServer.Core.Models;
using GaleonServer.Models.Queries;
using GaleonServer.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GaleonServer.Core.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, UserLoginResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenGateway _jwtTokenGateway;

    public LoginQueryHandler(UserManager<User> userManager, SignInManager<User> signInManager, IJwtTokenGateway jwtTokenGateway)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenGateway = jwtTokenGateway;
    }

    public async Task<UserLoginResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
                   ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (result.Succeeded is false)
            throw new HttpResponseException(HttpStatusCode.Unauthorized);

        var token = _jwtTokenGateway.CreateToken(user);

        return new UserLoginResponse
        {
            UserName = user.UserName,
            Token = token
        };
    }
}
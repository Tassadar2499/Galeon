using GaleonServer.Core.Models;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GaleonServer.Core.Commands;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, SimpleResponse>
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<SimpleResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return SimpleResponse.CreateError("Не найден пользователь с заданным email");
        
        var result = await _userManager.ResetPasswordAsync(user, request.Code, request.Password);
        
        if (result.Succeeded is false)
            return SimpleResponse.CreateError(result.Errors.Select(z => z.Description));
        
        return SimpleResponse.CreateSucceed();
    }
}
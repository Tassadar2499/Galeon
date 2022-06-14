using GaleonServer.Core.Models;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GaleonServer.Core.Commands;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, SimpleResponse>
{
    private readonly UserManager<User> _userManager;

    public ConfirmEmailCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<SimpleResponse> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return SimpleResponse.CreateError($"Не найден пользователь по id = {userId}");
        
        var result = await _userManager.ConfirmEmailAsync(user, request.Code);
        
        return result.Succeeded
            ? SimpleResponse.CreateSucceed()
            : SimpleResponse.CreateError(result.Errors.Select(z => z.Description));
    }
}
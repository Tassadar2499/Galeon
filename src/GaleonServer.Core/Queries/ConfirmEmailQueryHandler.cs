using GaleonServer.Core.Models;
using GaleonServer.Models.Queries;
using GaleonServer.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GaleonServer.Core.Queries;

public class ConfirmEmailQueryHandler : IRequestHandler<ConfirmEmailQuery, SimpleResponse>
{
    private readonly UserManager<User> _userManager;

    public ConfirmEmailQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<SimpleResponse> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
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
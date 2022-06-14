using GaleonServer.Core.Models;
using GaleonServer.Interfaces.Gateways;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Dto;
using GaleonServer.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GaleonServer.Core.Commands;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, SimpleResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailGateway _emailGateway;

    public ForgotPasswordCommandHandler(UserManager<User> userManager, IEmailGateway emailGateway)
    {
        _userManager = userManager;
        _emailGateway = emailGateway;
    }

    public async Task<SimpleResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var createCallbackUrl = request.CreateCallbackUrl;
        if (createCallbackUrl is null)
            throw new Exception("Create callback url func is not initialized");
        
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return SimpleResponse.CreateError("Не найден пользователь с заданным email");

        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        if (isEmailConfirmed is false)
            return SimpleResponse.CreateError("Необходимо подтвердить email");
        
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);

        var userCallbackUrlDto = new UserCallbackUrlDto
        {
            UserId = user.Id,
            Code = code
        };
        
        var callbackUrl = createCallbackUrl(userCallbackUrlDto);
        
        var message = $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>Сбросить пароль</a>";
        
        var emailDto = new SendEmailDto { Message = message, Email = request.Email };

        await _emailGateway.SendEmail(emailDto, cancellationToken);

        return SimpleResponse.CreateSucceed();
    }
}
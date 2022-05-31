using GaleonServer.Core.Models;
using GaleonServer.Interfaces.Gateways;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Dto;
using GaleonServer.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GaleonServer.Core.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IEmailGateway _emailGateway;
    private readonly UserManager<User> _userManager;

    public RegisterCommandHandler(IEmailGateway emailGateway, UserManager<User> userManager)
    {
        _emailGateway = emailGateway;
        _userManager = userManager;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var createCallbackUrl = request.CreateCallbackUrl;
        if (createCallbackUrl is null)
            throw new Exception("Create callback url func is not initialized");
        
        var email = request.Email;
        var userExist = await _userManager.FindByEmailAsync(email);

        if (userExist is not null)
            return CreateResponse("Данный пользователь уже зарегистрирован");
        
        var user = new User { Email = email, UserName = email };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded is false)
        {
            var errorsStr = string.Join(Environment.NewLine, result.Errors.Select(z => z.Description));

            return CreateResponse(errorsStr);
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var userCallbackUrlDto = new UserCallbackUrlDto
        {
            UserId = user.Id,
            Code = code
        };
        
        var callbackUrl = createCallbackUrl(userCallbackUrlDto);

        var message = $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>Подтвердить</a>";

        var emailDto = new SendEmailDto { Message = message, Email = email };

        await _emailGateway.SendEmail(emailDto, cancellationToken);

        return CreateResponse("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
    }

    private static RegisterResponse CreateResponse(string info)
    {
        return new()
        {
            Info = info
        };
    }
}
using System.Net;
using GaleonServer.Core.Exceptions;
using GaleonServer.Core.Gateways;
using GaleonServer.Core.Models;
using GaleonServer.Interfaces.Gateways;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Dto;
using GaleonServer.Models.Interfaces.Requests;
using GaleonServer.Models.Interfaces.Responses;
using GaleonServer.Models.Queries;
using GaleonServer.Models.Responses;
using Microsoft.AspNetCore.Identity;

namespace GaleonServer.Core.Services;

public interface IAuthorizationService
{
    public Task<TResult> Handle<TRequest, TResult>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IAuthorizationServiceRequest
        where TResult : IAuthorizationServiceResponse;
}

public class AuthorizationService : IAuthorizationService
{
    private readonly Lazy<UserManager<User>> _userManager;
    private readonly Lazy<SignInManager<User>> _signInManager;
    private readonly Lazy<IJwtTokenGateway> _jwtTokenGateway;
    private readonly Lazy<IEmailGateway> _emailGateway;

    public AuthorizationService(Lazy<UserManager<User>> userManager, Lazy<SignInManager<User>> signInManager, Lazy<IJwtTokenGateway> jwtTokenGateway, Lazy<IEmailGateway> emailGateway)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenGateway = jwtTokenGateway;
        _emailGateway = emailGateway;
    }

    public async Task<TResult> Handle<TRequest, TResult>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IAuthorizationServiceRequest
        where TResult : IAuthorizationServiceResponse
    {
        IAuthorizationServiceResponse result = request switch
        {
            LoginQuery loginQuery => await HandleLogin(loginQuery, cancellationToken),
            ConfirmEmailCommand confirmEmailCommand => await HandleConfirmEmail(confirmEmailCommand, cancellationToken),
            ForgotPasswordCommand forgotPasswordCommand => await HandleForgotPassword(forgotPasswordCommand, cancellationToken),
            RegisterCommand registerCommand => await HandleRegister(registerCommand, cancellationToken),
            ResetPasswordCommand resetPasswordCommand => await HandleResetPassword(resetPasswordCommand, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(request), request, null)
        };

        return (TResult) result;
    }
    
    private async Task<UserLoginResponse> HandleLogin(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Value.FindByEmailAsync(request.Email)
                   ?? throw new HttpResponseException(HttpStatusCode.Unauthorized);

        var emailConfirmed = await _userManager.Value.IsEmailConfirmedAsync(user);
        if (emailConfirmed is false)
            throw new HttpResponseException(HttpStatusCode.Unauthorized);

        var passwordResult = await _signInManager.Value.CheckPasswordSignInAsync(user, request.Password, false);
        if (passwordResult.Succeeded is false)
            throw new HttpResponseException(HttpStatusCode.Unauthorized);

        var token = _jwtTokenGateway.Value.CreateToken(user);

        return new ()
        {
            UserName = user.UserName,
            Token = token
        };
    }
    
    private async Task<SimpleResponse> HandleConfirmEmail(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var user = await _userManager.Value.FindByIdAsync(userId);
        if (user is null)
            return SimpleResponse.CreateError($"Не найден пользователь по id = {userId}");
        
        var result = await _userManager.Value.ConfirmEmailAsync(user, request.Code);
        
        return result.Succeeded
            ? SimpleResponse.CreateSucceed()
            : SimpleResponse.CreateError(result.Errors.Select(z => z.Description));
    }
    
    private async Task<SimpleResponse> HandleForgotPassword(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var createCallbackUrl = request.CreateCallbackUrl;
        if (createCallbackUrl is null)
            throw new Exception("Create callback url func is not initialized");
        
        var user = await _userManager.Value.FindByEmailAsync(request.Email);
        if (user is null)
            return SimpleResponse.CreateError("Не найден пользователь с заданным email");

        var isEmailConfirmed = await _userManager.Value.IsEmailConfirmedAsync(user);
        if (isEmailConfirmed is false)
            return SimpleResponse.CreateError("Необходимо подтвердить email");
        
        var code = await _userManager.Value.GeneratePasswordResetTokenAsync(user);

        var userCallbackUrlDto = new UserCallbackUrlDto
        {
            UserId = user.Id,
            Code = code
        };
        
        var callbackUrl = createCallbackUrl(userCallbackUrlDto);
        
        var message = $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>Сбросить пароль</a>";
        
        var emailDto = new SendEmailDto { Message = message, Email = request.Email };

        await _emailGateway.Value.SendEmail(emailDto, cancellationToken);

        return SimpleResponse.CreateSucceed();
    }
    
    private async Task<SimpleResponse> HandleRegister(RegisterCommand request, CancellationToken cancellationToken)
    {
        var createCallbackUrl = request.CreateCallbackUrl;
        if (createCallbackUrl is null)
            throw new Exception("Create callback url func is not initialized");
        
        var email = request.Email;
        var userExist = await _userManager.Value.FindByEmailAsync(email);

        if (userExist is not null)
            return SimpleResponse.CreateError("Данный пользователь уже зарегистрирован");
        
        var user = new User { Email = email, UserName = email };

        var result = await _userManager.Value.CreateAsync(user, request.Password);

        if (result.Succeeded is false)
            return SimpleResponse.CreateError(result.Errors.Select(z => z.Description));

        var code = await _userManager.Value.GenerateEmailConfirmationTokenAsync(user);

        var userCallbackUrlDto = new UserCallbackUrlDto
        {
            UserId = user.Id,
            Code = code
        };
        
        var callbackUrl = createCallbackUrl(userCallbackUrlDto);

        var message = $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>Подтвердить</a>";

        var emailDto = new SendEmailDto { Message = message, Email = email };

        await _emailGateway.Value.SendEmail(emailDto, cancellationToken);

        return SimpleResponse.CreateSucceed();
    }
    
    private async Task<SimpleResponse> HandleResetPassword(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Value.FindByEmailAsync(request.Email);
        if (user == null)
            return SimpleResponse.CreateError("Не найден пользователь с заданным email");
        
        var result = await _userManager.Value.ResetPasswordAsync(user, request.Code, request.Password);
        
        if (result.Succeeded is false)
            return SimpleResponse.CreateError(result.Errors.Select(z => z.Description));
        
        return SimpleResponse.CreateSucceed();
    }
}
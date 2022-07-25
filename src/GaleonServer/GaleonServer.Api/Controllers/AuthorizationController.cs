using System.Net;
using GaleonServer.Core.Exceptions;
using GaleonServer.Core.Services.Interfaces;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Dto;
using GaleonServer.Models.Queries;
using GaleonServer.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GaleonServer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResponse>> Login(LoginQuery query, CancellationToken cancellationToken)
    {
        try
        {
            return await _authorizationService.Handle<LoginQuery, UserLoginResponse>(query, cancellationToken);
        }
        catch (HttpResponseException ex)
        {
            if (ex.StatusCode is HttpStatusCode.Unauthorized)
                return Unauthorized();
        }

        return Problem();
    }

    [HttpPost("register")]
    public async Task<ActionResult<SimpleResponse>> Register(RegisterCommand command, CancellationToken cancellationToken)
    {
        command.SetCreateCallbackUrl(CreateConfirmEmailCallbackUrl);

        return await _authorizationService.Handle<RegisterCommand, SimpleResponse>(command, cancellationToken);
    }
    
    [HttpPost("forgot-password")]
    public async Task<ActionResult<SimpleResponse>> ForgotPassword(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        command.SetCreateCallbackUrl(CreateResetPasswordCheckCallbackUrl);

        return await _authorizationService.Handle<ForgotPasswordCommand, SimpleResponse>(command, cancellationToken);
    }
    
    [HttpPost("reset-password")]
    public async Task<ActionResult<SimpleResponse>> ResetPassword(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        return await _authorizationService.Handle<ResetPasswordCommand, SimpleResponse>(command, cancellationToken);
    }
    
    [HttpGet(nameof(ResetPasswordCheck))]
    public ActionResult<SimpleResponse> ResetPasswordCheck(string code)
    {
        return string.IsNullOrEmpty(code)
            ? SimpleResponse.CreateError("Code is empty")
            : SimpleResponse.CreateSucceed();
    }
    
    [HttpGet(nameof(ConfirmEmail))]
    public async Task<ActionResult<SimpleResponse>> ConfirmEmail([FromQuery] string userId, [FromQuery] string code, CancellationToken cancellationToken)
    {
        var command = new ConfirmEmailCommand
        {
            UserId = userId,
            Code = code
        };

        return await _authorizationService.Handle<ConfirmEmailCommand, SimpleResponse>(command, cancellationToken);
    }

    private string? CreateConfirmEmailCallbackUrl(UserCallbackUrlDto userCallbackUrlDto) => CreateCallbackUrl(userCallbackUrlDto, nameof(ConfirmEmail));
    private string? CreateResetPasswordCheckCallbackUrl(UserCallbackUrlDto userCallbackUrlDto) => CreateCallbackUrl(userCallbackUrlDto, nameof(ResetPasswordCheck));

    private string? CreateCallbackUrl(UserCallbackUrlDto userCallbackUrlDto, string action)
    {
        var urlValues = new { userCallbackUrlDto.UserId, userCallbackUrlDto.Code };
        var protocol = HttpContext.Request.Scheme;

        return Url.Action(action, "Authorization", urlValues, protocol);
    }
}
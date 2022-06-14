using System.Net;
using GaleonServer.Core.Exceptions;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Dto;
using GaleonServer.Models.Queries;
using GaleonServer.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GaleonServer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResponse>> Login(LoginQuery query)
    {
        //TODO: Отрефакторить с аттрибутами
        try
        {
            return await _mediator.Send(query);
        }
        catch (HttpResponseException ex)
        {
            if (ex.StatusCode is HttpStatusCode.Unauthorized)
                return Unauthorized();
        }

        return Problem();
    }

    [HttpPost("register")]
    public async Task<ActionResult<SimpleResponse>> Register(RegisterCommand command)
    {
        command.SetCreateCallbackUrl(CreateConfirmEmailCallbackUrl);
        
        return await _mediator.Send(command);
    }
    
    [HttpPost("forgot-password")]
    public async Task<ActionResult<SimpleResponse>> ForgotPassword(ForgotPasswordCommand command)
    {
        command.SetCreateCallbackUrl(CreateResetPasswordCheckCallbackUrl);

        return await _mediator.Send(command);
    }
    
    [HttpPost("reset-password")]
    public async Task<ActionResult<SimpleResponse>> ResetPassword(ResetPasswordCommand command)
    {
        return await _mediator.Send(command);
    }
    
    [HttpGet(nameof(ResetPasswordCheck))]
    public ActionResult<SimpleResponse> ResetPasswordCheck(string code)
    {
        return string.IsNullOrEmpty(code)
            ? SimpleResponse.CreateError("Code is empty")
            : SimpleResponse.CreateSucceed();
    }
    
    [HttpGet(nameof(ConfirmEmail))]
    public async Task<ActionResult<SimpleResponse>> ConfirmEmail(ConfirmEmailCommand command)
    {
        return await _mediator.Send(command);
    }

    private string? CreateConfirmEmailCallbackUrl(UserCallbackUrlDto userCallbackUrlDto) => CreateCallbackUrl(userCallbackUrlDto, nameof(ConfirmEmail));
    private string? CreateResetPasswordCheckCallbackUrl(UserCallbackUrlDto userCallbackUrlDto) => CreateCallbackUrl(userCallbackUrlDto, nameof(ResetPasswordCheck));

    private string? CreateCallbackUrl(UserCallbackUrlDto userCallbackUrlDto, string action)
    {
        var urlValues = new { userCallbackUrlDto.UserId, userCallbackUrlDto.Code };
        var protocol = HttpContext.Request.Scheme;

        return Url.Action(action, "User", urlValues, protocol);
    }
}
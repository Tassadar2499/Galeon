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
    
    [HttpGet]
    public async Task<ActionResult<SimpleResponse>> ConfirmEmail(ConfirmEmailQuery query)
    {
        return await _mediator.Send(query);
    }
    
    private string? CreateConfirmEmailCallbackUrl(UserCallbackUrlDto userCallbackUrlDto) => CreateCallbackUrl(userCallbackUrlDto, nameof(ConfirmEmail));

    private string? CreateCallbackUrl(UserCallbackUrlDto userCallbackUrlDto, string action)
    {
        var urlValues = new { userCallbackUrlDto.UserId, userCallbackUrlDto.Code };
        var protocol = HttpContext.Request.Scheme;

        return Url.Action(action, "User", urlValues, protocol);
    }
}
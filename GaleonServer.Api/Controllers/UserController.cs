using System.Net;
using GaleonServer.Core.Exceptions;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Queries;
using GaleonServer.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GaleonServer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMediator _mediator;

    public UserController(ILogger<UserController> logger, IMediator mediator)
    {
        _logger = logger;
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
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }

        return Problem();
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterCommand registerCommand)
    {
        try
        {
            await _mediator.Send(registerCommand);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            throw;
        }

        return Ok();
    }
}
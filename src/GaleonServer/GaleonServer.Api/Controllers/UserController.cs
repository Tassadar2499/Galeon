using GaleonServer.Models.Commands;
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

    [HttpDelete("delete")]
    public async Task DeleteUser(DeleteUserCommand command)
    {
        _ = await _mediator.Send(command);
    }
}
using GaleonServer.Core.Services.Interfaces;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GaleonServer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpDelete("delete")]
    public async Task<SimpleResponse> DeleteUser(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        return await _userService.Handle<DeleteUserCommand, SimpleResponse>(command, cancellationToken);
    }
}
using GaleonServer.Models.Responses;
using MediatR;

namespace GaleonServer.Models.Queries;

public class LoginQuery : IRequest<UserLoginResponse>
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}
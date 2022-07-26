using GaleonServer.Models.Interfaces.Requests;
using GaleonServer.Models.Responses;

namespace GaleonServer.Models.Queries;

public class LoginQuery : IAuthorizationServiceRequest<UserLoginResponse>
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}
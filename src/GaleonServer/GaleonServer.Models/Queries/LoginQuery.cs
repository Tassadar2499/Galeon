using GaleonServer.Models.Interfaces.Requests;

namespace GaleonServer.Models.Queries;

public class LoginQuery : IAuthorizationServiceRequest
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}
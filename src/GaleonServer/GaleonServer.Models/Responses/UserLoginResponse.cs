using GaleonServer.Models.Interfaces.Responses;

namespace GaleonServer.Models.Responses;

public class UserLoginResponse : IAuthorizationServiceResponse
{
    public string UserName { get; init; } = null!;
    public string Token { get; init; } = null!;
}
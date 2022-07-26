using GaleonServer.Models.Interfaces.Responses;

namespace GaleonServer.Models.Responses;

public class UserLoginResponse
{
    public string UserName { get; init; } = null!;
    public string Token { get; init; } = null!;
}
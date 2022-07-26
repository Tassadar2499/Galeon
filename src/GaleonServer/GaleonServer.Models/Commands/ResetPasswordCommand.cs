using GaleonServer.Models.Interfaces.Requests;
using GaleonServer.Models.Interfaces.Responses;
using GaleonServer.Models.Responses;

namespace GaleonServer.Models.Commands;

public class ResetPasswordCommand : IAuthorizationServiceRequest<SimpleResponse>
{
    //TODO: Add fluent validation
    public string Email { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
    public string Code { get; init; }
}
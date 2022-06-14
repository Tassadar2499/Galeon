using GaleonServer.Models.Responses;
using MediatR;

namespace GaleonServer.Models.Commands;

public class ResetPasswordCommand : IRequest<SimpleResponse>
{
    //TODO: Add fluent validation
    public string Email { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
    public string Code { get; init; }
}
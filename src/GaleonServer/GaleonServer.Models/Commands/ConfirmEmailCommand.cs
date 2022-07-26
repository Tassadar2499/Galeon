using GaleonServer.Models.Interfaces.Requests;
using GaleonServer.Models.Interfaces.Responses;

namespace GaleonServer.Models.Commands;

public class ConfirmEmailCommand : IAuthorizationServiceRequest
{
    public string UserId { get; init; }
    public string Code { get; init; }
}
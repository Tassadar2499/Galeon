using GaleonServer.Models.Interfaces.Requests;
using GaleonServer.Models.Interfaces.Responses;
using GaleonServer.Models.Responses;

namespace GaleonServer.Models.Commands;

public class ConfirmEmailCommand : IAuthorizationServiceRequest<SimpleResponse>
{
    public string UserId { get; init; }
    public string Code { get; init; }
}
using GaleonServer.Models.Responses;
using MediatR;

namespace GaleonServer.Models.Commands;

public class ConfirmEmailCommand : IRequest<SimpleResponse>
{
    public string UserId { get; init; }
    public string Code { get; init; }
}
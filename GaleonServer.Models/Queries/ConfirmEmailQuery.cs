using GaleonServer.Models.Responses;
using MediatR;

namespace GaleonServer.Models.Queries;

public class ConfirmEmailQuery : IRequest<SimpleResponse>
{
    public string UserId { get; init; }
    public string Code { get; init; }
}
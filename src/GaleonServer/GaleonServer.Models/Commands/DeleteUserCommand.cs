using MediatR;

namespace GaleonServer.Models.Commands;

public class DeleteUserCommand: IRequest
{
    public string Id { get; init; } = null!;
}
using MediatR;

namespace GaleonServer.Models.Commands;

public class AddGameCommand : IRequest
{
    public string Name { get; set; } = null!;
}
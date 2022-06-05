using GaleonServer.Core.Gateways;
using GaleonServer.Models.Commands;
using MediatR;

namespace GaleonServer.Core.Commands;

public class AddGameCommandHandler : IRequestHandler<AddGameCommand>
{
	private readonly IGameGateway _gameGateway;

	public AddGameCommandHandler(IGameGateway gameGateway)
	{
		_gameGateway = gameGateway;
	}

	public async Task<Unit> Handle(AddGameCommand request, CancellationToken cancellationToken)
	{
		await _gameGateway.Add(request.Name, cancellationToken);

		return new Unit();
	}
}
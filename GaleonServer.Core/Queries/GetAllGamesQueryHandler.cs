using GaleonServer.Core.Gateways;
using GaleonServer.Models.Queries;
using GaleonServer.Models.Responses;
using MediatR;

namespace GaleonServer.Core.Queries;

public class GetAllGamesQueryHandler : IStreamRequestHandler<GetAllGamesQuery, GameResponse>
{
	private readonly IGameReadonlyGateway _gameGateway;

	public GetAllGamesQueryHandler(IGameReadonlyGateway gameGateway)
	{
		_gameGateway = gameGateway;
	}

	public IAsyncEnumerable<GameResponse> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
	{
		return _gameGateway.GetAll(cancellationToken);
	}
}
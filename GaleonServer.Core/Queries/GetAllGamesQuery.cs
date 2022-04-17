using GaleonServer.Core.Dto;
using GaleonServer.Core.Gateways;
using MediatR;

namespace GaleonServer.Core.Queries
{
	public class GetAllGamesQuery : IStreamRequest<GameDto>
	{
	}

	public class GetAllGamesQueryHandler : IStreamRequestHandler<GetAllGamesQuery, GameDto>
	{
		private readonly IGameReadonlyGateway _gameGateway;

		public GetAllGamesQueryHandler(IGameReadonlyGateway gameGateway)
		{
			_gameGateway = gameGateway;
		}

		public IAsyncEnumerable<GameDto> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
		{
			return _gameGateway.GetAll(cancellationToken);
		}
	}
}
using GaleonServer.Core.Dto;
using GaleonServer.Core.Gateways;
using MediatR;

namespace GaleonServer.Core.Queries
{
	public class GetAllGamesQuery : IRequest<IReadOnlyCollection<GameDto>>
	{
	}

	public class GetAllGamesQueryHandler : IRequestHandler<GetAllGamesQuery, IReadOnlyCollection<GameDto>>
	{
		private readonly IGameGateway _gameGateway;

		public GetAllGamesQueryHandler(IGameGateway gameGateway)
		{
			_gameGateway = gameGateway;
		}

		public async Task<IReadOnlyCollection<GameDto>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
		{
			return await _gameGateway.GetAll(cancellationToken);
		}
	}
}
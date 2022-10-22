using GaleonServer.Core.Gateways;
using GaleonServer.Infrastructure.Database;
using GaleonServer.Models.Responses;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace GaleonServer.Infrastructure.Gateways
{
	public class GameReadonlyGateway : IGameReadonlyGateway
	{
		private readonly GaleonReadonlyContext _context;

		public GameReadonlyGateway(GaleonReadonlyContext context)
		{
			_context = context;
		}

		public IAsyncEnumerable<GameResponse> GetAll(CancellationToken cancellationToken)
		{
			return _context.Games
				.ProjectToType<GameResponse>()
				.AsAsyncEnumerable();
		}
	}
}
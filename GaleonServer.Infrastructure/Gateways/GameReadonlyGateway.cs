using GaleonServer.Core.Dto;
using GaleonServer.Core.Gateways;
using GaleonServer.Infrastructure.Database;
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

		public async Task<IReadOnlyCollection<GameDto>> GetAll(CancellationToken cancellationToken)
		{
			return await _context.Games
				.Select(x => new GameDto() { Name = x.Name })
				.ToArrayAsync(cancellationToken);
		}
	}
}
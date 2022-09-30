using GaleonServer.Core.Gateways;
using GaleonServer.Core.Models;
using GaleonServer.Infrastructure.Database;
using GaleonServer.Interfaces.Gateways;
using GaleonServer.Models.Flags;
using GaleonServer.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace GaleonServer.Infrastructure.Gateways
{
	public class GameGateway : IGameGateway
	{
		private readonly GaleonContext _context;

		public GameGateway(GaleonContext context)
		{
			_context = context;
		}

		public async ValueTask<Game?> Get(int id, GameFetchOptions gameFetchOptions, CancellationToken cancellationToken)
		{
			IQueryable<Game> query = _context.Games;
			
			if (gameFetchOptions.HasFlag(GameFetchOptions.MagnetLinks))
				query = query.Include(z => z.MagnetLinks);
			
			return await query.FirstOrDefaultAsync(z => z.Id == id, cancellationToken: cancellationToken);
		}

		public async ValueTask Create(Game game, CancellationToken cancellationToken)
		{
			_context.Games.Add(game);
			await SaveChanges(cancellationToken);
		}

		public async ValueTask SaveChanges(CancellationToken cancellationToken)
		{
			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
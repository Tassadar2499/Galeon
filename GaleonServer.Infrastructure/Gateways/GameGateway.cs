using GaleonServer.Core.Gateways;
using GaleonServer.Core.Models;
using GaleonServer.Infrastructure.Database;

namespace GaleonServer.Infrastructure.Gateways
{
	public class GameGateway : IGameGateway
	{
		private readonly GaleonContext _context;

		public GameGateway(GaleonContext context)
		{
			_context = context;
		}

		public async Task Add(string name, CancellationToken cancellationToken)
		{
			var game = new Game() { Name = name };

			await _context.AddAsync(game, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
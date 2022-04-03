using GaleonServer.Core.Dto;
using GaleonServer.Core.Gateways;
using GaleonServer.Core.Models;
using GaleonServer.Infrastructure.Database;
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

		public async Task Add(string name)
		{
			var game = new Game() { Name = name };

			await _context.AddAsync(game);
			await _context.SaveChangesAsync();
		}

		public async Task<IReadOnlyCollection<GameDto>> GetAll(CancellationToken cancellationToken)
		{
			return await _context.Games
				.Select(x => new GameDto { Name = x.Name })
				.ToArrayAsync(cancellationToken);
		}
	}
}
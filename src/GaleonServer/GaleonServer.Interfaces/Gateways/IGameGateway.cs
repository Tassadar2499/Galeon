using GaleonServer.Models.Flags;
using GaleonServer.Models.Models;

namespace GaleonServer.Interfaces.Gateways
{
	public interface IGameGateway
	{
		ValueTask<Game?> Get(int id, GameFetchOptions gameFetchOptions, CancellationToken cancellationToken);
		ValueTask Create(Game game, CancellationToken cancellationToken);
		ValueTask SaveChanges(CancellationToken cancellationToken);
	}
}
using GaleonServer.Models.Responses;

namespace GaleonServer.Core.Gateways
{
	public interface IGameReadonlyGateway
	{
		IAsyncEnumerable<GameResponse> GetAll(CancellationToken cancellationToken);
	}
}
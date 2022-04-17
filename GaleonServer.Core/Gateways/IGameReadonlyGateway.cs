using GaleonServer.Core.Dto;

namespace GaleonServer.Core.Gateways
{
	public interface IGameReadonlyGateway
	{
		IAsyncEnumerable<GameDto> GetAll(CancellationToken cancellationToken);
	}
}
using GaleonServer.Core.Dto;

namespace GaleonServer.Core.Gateways
{
	public interface IGameReadonlyGateway
	{
		Task<IReadOnlyCollection<GameDto>> GetAll(CancellationToken cancellationToken);
	}
}
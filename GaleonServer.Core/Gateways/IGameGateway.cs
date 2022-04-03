using GaleonServer.Core.Dto;

namespace GaleonServer.Core.Gateways
{
	public interface IGameGateway
	{
		Task<IReadOnlyCollection<GameDto>> GetAll(CancellationToken cancellationToken);

		Task Add(string name);
	}
}
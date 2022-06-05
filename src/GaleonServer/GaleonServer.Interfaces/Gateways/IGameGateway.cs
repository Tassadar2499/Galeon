namespace GaleonServer.Core.Gateways
{
	public interface IGameGateway
	{
		Task Add(string name, CancellationToken cancellationToken);
	}
}
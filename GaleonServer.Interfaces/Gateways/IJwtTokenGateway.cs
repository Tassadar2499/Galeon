using GaleonServer.Core.Models;

namespace GaleonServer.Core.Gateways
{
	public interface IJwtTokenGateway
	{
		string CreateToken(User user);
	}
}

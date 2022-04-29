using GaleonServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaleonServer.Core.Gateways
{
	public interface IJwtTokenGateway
	{
		string CreateToken(User user);
	}
}

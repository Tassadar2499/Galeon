using GaleonServer.Core.Gateways;
using GaleonServer.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using GaleonServer.Models.Options;
using Microsoft.Extensions.Options;

namespace GaleonServer.Infrastructure.Gateways
{
	public class JwtTokenGateway : IJwtTokenGateway
	{
		private readonly string _tokenKey;

		public JwtTokenGateway(IOptions<IdentityOptions> options)
		{
			_tokenKey = options.Value.TokenKey;
		}

		public string CreateToken(User user)
		{
			var bytes = Encoding.UTF8.GetBytes(_tokenKey);
			var key = new SymmetricSecurityKey(bytes);

			var claims = new Claim []
			{
				new (JwtRegisteredClaimNames.NameId, user.UserName)
			};

			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
			var expiredDate = DateTime.UtcNow.AddDays(7);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new (claims),
				Expires = expiredDate,
				SigningCredentials = credentials
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}

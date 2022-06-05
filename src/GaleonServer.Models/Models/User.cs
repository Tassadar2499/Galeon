using Microsoft.AspNetCore.Identity;

namespace GaleonServer.Core.Models
{
	public class User : IdentityUser, IEntityBase<string>
	{
		public IReadOnlyCollection<UserToGame> UserGames { get; set; } = new List<UserToGame>();
	}
}
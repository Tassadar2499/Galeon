namespace GaleonServer.Core.Models
{
	public class User : EntityBase<int>
	{
		public string Login { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public IReadOnlyCollection<UserToGame> UserGames { get; set; } = new List<UserToGame>();
	}
}
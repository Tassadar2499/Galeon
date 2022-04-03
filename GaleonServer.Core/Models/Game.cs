namespace GaleonServer.Core.Models
{
	public class Game : EntityBase<int>
	{
		public string Name { get; set; } = null!;
		public IReadOnlyCollection<UserToGame> GameUsers { get; set; } = new List<UserToGame>();
		public IReadOnlyCollection<MagnetLink> MagnetLinks { get; set; } = new List<MagnetLink>();
	}
}
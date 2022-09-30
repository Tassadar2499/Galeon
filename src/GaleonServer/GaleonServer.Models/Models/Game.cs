using GaleonServer.Core.Models;

namespace GaleonServer.Models.Models
{
	public class Game : IEntityBase<int>
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public IReadOnlyCollection<UserToGame> GameUsers { get; set; } = new List<UserToGame>();
		public ICollection<MagnetLink> MagnetLinks { get; set; } = new List<MagnetLink>();
	}
}
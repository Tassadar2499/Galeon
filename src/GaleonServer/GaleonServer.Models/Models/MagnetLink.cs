using GaleonServer.Models.Models;

namespace GaleonServer.Core.Models
{
	public class MagnetLink : IEntityBase<int>
	{
		public int Id { get; set; }
		public int GameId { get; set; }
		public string Value { get; set; } = null!;
		public Game Game { get; set; } = null!;
	}
}
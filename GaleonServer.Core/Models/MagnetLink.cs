namespace GaleonServer.Core.Models
{
	public class MagnetLink : EntityBase<int>
	{
		public int GameId { get; set; }
		public string Value { get; set; } = null!;
		public Game Game { get; set; } = null!;
	}
}
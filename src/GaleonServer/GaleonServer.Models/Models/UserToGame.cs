namespace GaleonServer.Core.Models
{
	public class UserToGame : IEntityBase<int>
	{
		public int Id { get; set; }
		public int GameId { get; set; }
		public int UserId { get; set; }
		public Game Game { get; set; } = null!;
		public User User { get; set; } = null!;
	}
}
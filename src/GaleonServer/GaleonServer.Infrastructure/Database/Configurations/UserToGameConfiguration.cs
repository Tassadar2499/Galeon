using GaleonServer.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GaleonServer.Infrastructure.Database.Configurations
{
	public class UserToGameConfiguration : IEntityTypeConfiguration<UserToGame>
	{
		public void Configure(EntityTypeBuilder<UserToGame> builder)
		{
			builder.HasKey(x => x.Id);

			builder
				.HasOne(x => x.User)
				.WithMany(x => x.UserGames)
				.HasForeignKey(x => x.UserId);

			builder
				.HasOne(x => x.Game)
				.WithMany(x => x.GameUsers)
				.HasForeignKey(x => x.GameId);
		}
	}
}
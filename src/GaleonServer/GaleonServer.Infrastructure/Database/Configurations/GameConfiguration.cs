using GaleonServer.Core.Models;
using GaleonServer.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GaleonServer.Infrastructure.Database.Configurations
{
	public class GameConfiguration : IEntityTypeConfiguration<Game>
	{
		public void Configure(EntityTypeBuilder<Game> builder)
		{
			builder.HasKey(x => x.Id);

			builder
				.HasMany<UserToGame>()
				.WithOne(x => x.Game)
				.HasPrincipalKey(x => x.Id);
		}
	}
}
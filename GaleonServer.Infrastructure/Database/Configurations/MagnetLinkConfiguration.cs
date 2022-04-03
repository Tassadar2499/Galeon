using GaleonServer.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GaleonServer.Infrastructure.Database.Configurations
{
	internal class MagnetLinkConfiguration : IEntityTypeConfiguration<MagnetLink>
	{
		public void Configure(EntityTypeBuilder<MagnetLink> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasOne(x => x.Game)
				.WithMany(x => x.MagnetLinks)
				.HasForeignKey(x => x.GameId);
		}
	}
}
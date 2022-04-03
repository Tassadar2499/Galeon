using GaleonServer.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GaleonServer.Infrastructure.Database.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<MagnetLink>
	{
		public void Configure(EntityTypeBuilder<MagnetLink> builder)
		{
			builder.HasKey(x => x.Id);
		}
	}
}
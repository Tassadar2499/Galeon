using GaleonServer.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GaleonServer.Infrastructure.Database
{
	public class GaleonContext : IdentityDbContext<User>
	{
		public DbSet<Game> Games { get; set; } = null!;
		public DbSet<MagnetLink> MagnetLinks { get; set; } = null!;
		public DbSet<UserToGame> UsersToGames { get; set; } = null!;

		public GaleonContext(DbContextOptions<GaleonContext> options): this(options as DbContextOptions)
		{
		}

		protected GaleonContext(DbContextOptions options) : base(options)
		{
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(GaleonContext).Assembly);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder
				.UseNpgsql()
				.UseSnakeCaseNamingConvention();
		}
	}
}
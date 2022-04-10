using GaleonServer.Core.Gateways;
using GaleonServer.Infrastructure.Database;
using GaleonServer.Infrastructure.Gateways;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GaleonServer.Infrastructure
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("GaleonServerConection");

			services
				.AddEntityFrameworkNpgsql()
				.AddDbContext<GaleonContext>(opt => opt.UseNpgsql(connectionString));

			services
				.AddEntityFrameworkNpgsql()
				.AddDbContext<GaleonReadonlyContext>(opt => opt.UseNpgsql(connectionString));

			services.AddTransient<IGameGateway, GameGateway>();
			services.AddTransient<IGameReadonlyGateway, GameReadonlyGateway>();

			return services;
		}
	}
}
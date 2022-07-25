using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using GaleonServer.Core._Common;
using GaleonServer.Core.Services;
using GaleonServer.Core.Services.Interfaces;

namespace GaleonServer.Core
{
	public static class ServiceCollectionExtensions
	{
		public static void AddCore(this IServiceCollection services)
		{
			services.AddTransient(typeof(Lazy<>), typeof(Lazier<>));
			
			services.AddTransient<IAuthorizationService, AuthorizationService>();
			services.AddTransient<IGameService, GameService>();
			services.AddTransient<IUserService, UserService>();

			services.AddMediatR(Assembly.GetExecutingAssembly());
		}

		public static IServiceCollection ConfigureFromSection<T>(this IServiceCollection services, IConfiguration configuration, string path)
			where T : class
		{
			services.Configure<T>(z => configuration.GetSection(path).Bind(z));

			return services;
		}
	}
}
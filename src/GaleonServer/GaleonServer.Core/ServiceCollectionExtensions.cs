using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GaleonServer.Core
{
	public static class ServiceCollectionExtensions
	{
		public static void AddCore(this IServiceCollection services)
		{
			services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
		}

		public static IServiceCollection ConfigureFromSection<T>(this IServiceCollection services, IConfiguration configuration, string path)
			where T : class
		{
			services.Configure<T>(z => configuration.GetSection(path).Bind(z));

			return services;
		}
	}
}
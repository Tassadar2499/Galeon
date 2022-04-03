using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GaleonServer.Core
{
	public static class ServiceCollectionExtensions
	{
		public static void AddCore(this IServiceCollection services)
		{
			services.AddMediatR(typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly);
		}
	}
}
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GaleonServer.ContractTests._Common;

public static class Extensions
{
    public static async Task<TResponse> SendRequestByMediator<TRequest, TResponse>(this IntegrationTestWebApplicationFactory factory, TRequest request)
    {
        using var sp = factory.Server.Services.CreateScope();
        var mediator = sp.ServiceProvider.GetService<IMediator>()!;

        var response = await mediator.Send(request!);

        return (TResponse) response!;
    }

    public static async Task SendRequestByMediator<TRequest>(this IntegrationTestWebApplicationFactory factory, TRequest request)
    {
        using var sp = factory.Server.Services.CreateScope();
        var mediator = sp.ServiceProvider.GetService<IMediator>()!;

        _ = await mediator.Send(request!);
    }
    
    public static void RemoveDbContext<T>(this IServiceCollection services) where T : DbContext
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<T>));
        if (descriptor != null) services.Remove(descriptor);
    }

    public static void EnsureDbCreated<T>(this IServiceCollection services) where T : DbContext
    {
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<T>();
        context.Database.EnsureCreated();
    }
}
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GaleonServer.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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
        if (descriptor is not null)
            services.Remove(descriptor);
    }

    public static void RemoveService<T>(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
        if (descriptor is not null)
            services.Remove(descriptor);
    }

    public static void EnsureDbCreated<T>(this IServiceCollection services) where T : DbContext
    {
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<T>();
        context.Database.EnsureCreated();
    }

    public static void RecreateDatabase(this IntegrationTestWebApplicationFactory factory)
    {
        using var sp = factory.Server.Services.CreateScope();
        var context = sp.ServiceProvider.GetRequiredService<GaleonContext>();
        var database = context.Database;
        database.EnsureDeleted();
        database.EnsureCreated();
    }

    public static async Task<T> ReadRequest<T>(this string path)
    {
        var text = await File.ReadAllTextAsync(path);
        
        return JsonConvert.DeserializeObject<T>(text);
    }
}
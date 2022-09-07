using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace GaleonServer.BehaviourContractTests._Common;

public static class Extensions
{
    public static async Task<T> ReadRequest<T>(this string path)
    {
        var text = await File.ReadAllTextAsync(path);
        
        return JsonConvert.DeserializeObject<T>(text)!;
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
}
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public static async Task RecreateDatabase(this GaleonContext context)
    {
        var database = context.Database;
        await database.EnsureDeletedAsync();
        await database.EnsureCreatedAsync();
    }
    
    public static string GetCurrentMethodName([CallerMemberName] string name = "") => name;

    public static async Task<T> ReadRequest<T>(this string path)
    {
        var text = await File.ReadAllTextAsync(path);
        
        return JsonConvert.DeserializeObject<T>(text)!;
    }
}
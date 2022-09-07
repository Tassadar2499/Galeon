using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using GaleonServer.BehaviourContractTests.Stubs;
using GaleonServer.Infrastructure.Database;
using GaleonServer.Interfaces.Gateways;
using GaleonServer.Models.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GaleonServer.BehaviourContractTests._Common;

public static class IntegrationTestWebApplicationFactorySingleton
{
    public static IntegrationTestWebApplicationFactory Instance => new ();
}

public class IntegrationTestWebApplicationFactory : IntegrationTestWebApplicationFactory<Program>
{
}

public class IntegrationTestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    private readonly TestcontainerDatabase _container;

    protected IntegrationTestWebApplicationFactory()
    {
        _container = InitTestContainer();
        
        var startTask = Task.Run(() => _container.StartAsync());
        startTask.Wait();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureTestServices);
    }

    private void ConfigureTestServices(IServiceCollection services)
    {
        services.Configure<IdentityOptions>(opts => opts.TokenKey = "MisterPepeVPepeStane");
        
        services.RemoveDbContext<GaleonContext>();
        services.AddDbContext<GaleonContext>(options => { options.UseNpgsql(_container.ConnectionString); });
        services.EnsureDbCreated<GaleonContext>();

        services.RemoveDbContext<GaleonReadonlyContext>();
        services.AddDbContext<GaleonReadonlyContext>(options => { options.UseNpgsql(_container.ConnectionString); });

        services.RemoveService<IEmailGateway>();
        services.AddTransient<IEmailGateway, EmailGatewayStub>();
    }

    private static TestcontainerDatabase InitTestContainer()
    {
        var builder = new TestcontainersBuilder<PostgreSqlTestcontainer>();
        var configuration = new PostgreSqlTestcontainerConfiguration
        {
            Database = "test_db",
            Username = "postgres",
            Password = "postgres",
            Port = 5434
        };
        
        return builder.WithDatabase(configuration)
            .WithImage("postgres:11")
            .WithCleanUp(true)
            .Build();
    }
}
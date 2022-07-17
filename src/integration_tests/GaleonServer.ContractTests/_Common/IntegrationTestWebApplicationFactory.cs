using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using GaleonServer.ContractTests.Stubs;
using GaleonServer.Infrastructure.Database;
using GaleonServer.Interfaces.Gateways;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GaleonServer.ContractTests._Common;

[CollectionDefinition(nameof(IntegrationTestsCollection))]
public class IntegrationTestsCollection: ICollectionFixture<IntegrationTestWebApplicationFactory>
{
}

public class IntegrationTestWebApplicationFactory : IntegrationTestWebApplicationFactory<Program>
{
}

public class IntegrationTestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
{
    private readonly TestcontainerDatabase _container;
    
    public IntegrationTestWebApplicationFactory()
    {
        _container = InitTestContainer();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureTestServices);
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public new async Task DisposeAsync() => await _container.DisposeAsync();

    private void ConfigureTestServices(IServiceCollection services)
    {
        services.RemoveDbContext<GaleonContext>();
        services.AddDbContext<GaleonContext>(options => { options.UseNpgsql(_container.ConnectionString); });
        services.EnsureDbCreated<GaleonContext>();
            
        services.RemoveDbContext<GaleonReadonlyContext>();
        services.AddDbContext<GaleonReadonlyContext>(options => { options.UseNpgsql(_container.ConnectionString); });
        services.EnsureDbCreated<GaleonReadonlyContext>();
            
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
        };
        
        return builder.WithDatabase(configuration)
            .WithImage("postgres:11")
            .WithCleanUp(true)
            .Build();
    }
}
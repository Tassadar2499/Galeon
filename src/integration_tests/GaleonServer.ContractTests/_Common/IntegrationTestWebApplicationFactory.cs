using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using GaleonServer.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
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
        _container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "test_db",
                Username = "postgres",
                Password = "postgres",
            })
            .WithImage("postgres:11")
            .WithCleanUp(true)
            .Build();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveDbContext<GaleonContext>();
            services.AddDbContext<GaleonContext>(options => { options.UseNpgsql(_container.ConnectionString); });
            services.EnsureDbCreated<GaleonContext>();
            services.RemoveDbContext<GaleonReadonlyContext>();
            services.AddDbContext<GaleonReadonlyContext>(options => { options.UseNpgsql(_container.ConnectionString); });
            services.EnsureDbCreated<GaleonReadonlyContext>();
        });
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public new async Task DisposeAsync() => await _container.DisposeAsync();
}
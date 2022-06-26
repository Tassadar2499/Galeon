using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FluentAssertions;
using GaleonServer.ContractTests._Common;
using GaleonServer.Core.Gateways;
using GaleonServer.Models.Commands;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace GaleonServer.ContractTests;

[Collection(nameof(IntegrationTestsCollection))]
public class AuthorizationTests
{
    private static string RequestsPath => $"{Consts.DefaultRequestPath}/{nameof(AuthorizationTests)}";
    
    private readonly IntegrationTestWebApplicationFactory _factory;
    public AuthorizationTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async void Kek()
    {
        var sp = _factory.Server.Services.CreateScope();
        var gameGateway = sp.ServiceProvider.GetService<IGameGateway>();
        var readonlyGateway = sp.ServiceProvider.GetService<IGameReadonlyGateway>();
        await gameGateway.Add("kek", CancellationToken.None);

        var games = new List<string>();
        await foreach (var game in readonlyGateway.GetAll(CancellationToken.None))
        {
            games.Add(game.Name);
        }

        games.Single().Should().Be("kek");
    }

    private static T GetRequest<T>(string methodName)
    {
        var requestFilePath = $"{RequestsPath}/{methodName}.json";
        var text = File.ReadAllText(requestFilePath);
        var request = JsonConvert.DeserializeObject<T>(text);

        return request;
    }
}
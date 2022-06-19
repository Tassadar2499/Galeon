using System.IO;
using GaleonServer.ContractTests._Common;
using GaleonServer.Models.Commands;
using Newtonsoft.Json;
using Xunit;

namespace GaleonServer.ContractTests;

public class AuthorizationTests
{
    private static string RequestsPath => $"{Consts.DefaultRequestPath}/{nameof(AuthorizationTests)}";
    
    private readonly IntegrationTestWebApplicationFactory _factory;
    public AuthorizationTests(IntegrationTestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public void Check_registration()
    {
        var command = GetRequest<RegisterCommand>(nameof(Check_registration));
    }

    private static T GetRequest<T>(string methodName)
    {
        var requestFilePath = $"{RequestsPath}/{methodName}.json";
        var text = File.ReadAllText(requestFilePath);
        var request = JsonConvert.DeserializeObject<T>(text);

        return request;
    }
}
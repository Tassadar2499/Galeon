using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GaleonServer.ContractTests._Common;
using GaleonServer.ContractTests.Stubs;
using GaleonServer.Core.Gateways;
using GaleonServer.Infrastructure.Database;
using GaleonServer.Interfaces.Gateways;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Dto;
using GaleonServer.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace GaleonServer.ContractTests;

[Collection(nameof(IntegrationTestsCollection))]
public class AuthorizationTests
{
    private static string RequestsPath => $"{Consts.DefaultRequestPath}/{nameof(AuthorizationTests)}";
    
    private readonly HttpClient _httpClient;
    private readonly GaleonContext _galeonContext;
    public AuthorizationTests(IntegrationTestWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
        
        var sp = factory.Server.Services.CreateScope();
        _galeonContext = sp.ServiceProvider.GetRequiredService<GaleonContext>();
    }

    [Fact]
    public async void Check_registration()
    {
        //arrange
        var methodName = Extensions.GetCurrentMethodName();
        var request = await GetRequest<RegisterCommand>(methodName);
        await _galeonContext.RecreateDatabase();

        //act
        var responseMessage = await _httpClient.PostAsJsonAsync("api/authorization/register", request);
        var responseStr = await responseMessage.Content.ReadAsStringAsync();
        
        //assert
        responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var response = JsonConvert.DeserializeObject<SimpleResponse>(responseStr);
        response?.Succeed.Should().BeTrue();
        
        EmailGatewayStub.EmailGateway.Verify(z => z.SendEmail(It.Is<SendEmailDto>(x => x.Email == request.Email), It.IsAny<CancellationToken>()), Times.Once);
        
        var user = await _galeonContext.Users.SingleOrDefaultAsync(z => z.Email == request.Email);
        user.Should().NotBeNull();
    }

    private static async Task<T> GetRequest<T>(string methodName)
    {
        var requestFilePath = $"{RequestsPath}/{methodName}.json";

        return await requestFilePath.ReadRequest<T>();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;
using GaleonServer.BehaviourContractTests._Common;
using GaleonServer.BehaviourContractTests.Stubs;
using GaleonServer.Infrastructure.Database;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Dto;
using GaleonServer.Models.Queries;
using GaleonServer.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using Xunit;

namespace GaleonServer.BehaviourContractTests.Steps;

[Binding]
public sealed class AuthorizationStepsDefinition
{
    private readonly ScenarioContext _scenarioContext;
    private readonly HttpClient _httpClient;
    private readonly GaleonContext _galeonContext;

    private RegisterCommand _registerRequest;
    private HttpResponseMessage _registerResponseMessage;
    private SimpleResponse? _registerResponse;
    
    private HttpResponseMessage _confirmEmailResponseMessage;
    private SimpleResponse? _confirmEmailResponse;
    
    private HttpResponseMessage _loginResponseMessage;
    private UserLoginResponse? _loginResponse;
    
    public AuthorizationStepsDefinition(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;

        var factory = IntegrationTestWebApplicationFactorySingleton.Instance;
        _httpClient = factory.CreateClient();
        
        var sp = factory.Server.Services.CreateScope();
        _galeonContext = sp.ServiceProvider.GetRequiredService<GaleonContext>();
    }

    [Given(@"request registration '(.*)'")]
    public async Task GivenRequestRegistration(string requestFilePath)
    {
        _registerRequest = await requestFilePath.ReadRequest<RegisterCommand>();
    }

    [When(@"registration requested")]
    public async Task WhenRegistrationRequested()
    {
        _registerResponseMessage = await _httpClient.PostAsJsonAsync("api/authorization/register", _registerRequest);
        var responseStr = await _registerResponseMessage.Content.ReadAsStringAsync();
        _registerResponse = JsonConvert.DeserializeObject<SimpleResponse>(responseStr);
    }
    
    [When(@"confirm email requested")]
    public async Task WhenConfirmEmailRequested()
    {
        var linkToRedirect = GetSendEmailLinkToRedirect();
        _confirmEmailResponseMessage = await _httpClient.GetAsync(linkToRedirect);
        var responseStr = await _confirmEmailResponseMessage.Content.ReadAsStringAsync();
        _confirmEmailResponse = JsonConvert.DeserializeObject<SimpleResponse>(responseStr);
    }

    [When(@"login requested")]
    public async Task WhenLoginRequested()
    {
        var request = new LoginQuery { Email = _registerRequest.Email, Password = _registerRequest.Password };
        _loginResponseMessage = await _httpClient.PostAsJsonAsync("api/authorization/login", request);
        var responseStr = await _loginResponseMessage.Content.ReadAsStringAsync();
        _loginResponse = JsonConvert.DeserializeObject<UserLoginResponse>(responseStr);
    }

    [Then(@"registration response status code should be (.*)")]
    public void ThenRegistrationResponseStatusCodeShouldBe(HttpStatusCode code)
    {
        _registerResponseMessage.StatusCode.Should().Be(code);
    }

    [Then(@"registration response succeed should be (.*)")]
    public void ThenRegistrationResponseSucceedShouldBeTrue(bool result)
    {
        _registerResponse!.Succeed.Should().Be(result);
    }

    [Then(@"user should be added to database")]
    public async Task ThenUserShouldBeAddedToDatabase()
    {
        var user = await _galeonContext.Users.SingleOrDefaultAsync(z => z.Email == _registerRequest.Email);
        user.Should().NotBeNull();
    }
    
    [Then(@"email to user email should be sent")]
    public void ThenEmailToUserEmailShouldBeSent()
    {
        EmailGatewayStub.EmailGateway.Verify(z => z.SendEmail(It.Is<SendEmailDto>(x => x.Email == _registerRequest.Email), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Then(@"confirm email response status code should be (.*)")]
    public void ThenConfirmEmailResponseStatusCodeShouldBe(HttpStatusCode code)
    {
        _confirmEmailResponseMessage.StatusCode.Should().Be(code);
    }

    [Then(@"confirm email response succeed should be (.*)")]
    public void ThenConfirmEmailResponseSucceedShouldBeTrue(bool result)
    {
        _confirmEmailResponse!.Succeed.Should().Be(result);
    }
    
    [Then(@"login response status code should be (.*)")]
    public void ThenLoginResponseStatusCodeShouldBe(HttpStatusCode code)
    {
        _loginResponseMessage.StatusCode.Should().Be(code);
    }

    [Then(@"login response token should not be empty")]
    public void ThenLoginResponseTokenShouldNotBeEmpty()
    {
        _loginResponse!.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Then(@"login response UserName should equals to user email")]
    public void ThenLoginResponseUserNameShouldEqualsToUserEmail()
    {
        _loginResponse?.UserName.Should().Be(_registerRequest.Email);
    }
    
    private static string GetSendEmailLinkToRedirect()
    {
        var sendEmailCall = EmailGatewayStub.GetLastSendEmailCall();
        var regex = new Regex(@"href=([""'])(.*?)\1");
        var match = regex.Match(sendEmailCall.Message);
        var subStr = match.ValueSpan[6..^1];

        return subStr.ToString();
    }
}
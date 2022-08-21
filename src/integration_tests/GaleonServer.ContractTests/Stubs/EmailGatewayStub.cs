using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GaleonServer.Interfaces.Gateways;
using GaleonServer.Models.Dto;
using Moq;

namespace GaleonServer.ContractTests.Stubs;

public class EmailGatewayStub : IEmailGateway
{
    public static Mock<IEmailGateway> EmailGateway { get; private set; }
    private static readonly Stack<SendEmailDto> SendEmailCalls = new();

    static EmailGatewayStub()
    {
        EmailGateway = Init();
    }
    
    public static SendEmailDto GetLastSendEmailCall() => SendEmailCalls.Pop();

    public EmailGatewayStub()
    {
        EmailGateway = Init();
    }

    public Task SendEmail(SendEmailDto sendEmailDto, CancellationToken cancellationToken)
    {
        SendEmailCalls.Push(sendEmailDto);
        return EmailGateway.Object.SendEmail(sendEmailDto, cancellationToken);
    }

    private static Mock<IEmailGateway> Init()
    {
        return new ()
        {
            CallBase = true
        };
    }
}
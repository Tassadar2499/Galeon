using GaleonServer.Interfaces.Gateways;
using GaleonServer.Models.Dto;
using GaleonServer.Models.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace GaleonServer.Infrastructure.Gateways;

public class EmailGateway : IEmailGateway
{
    private const string MainAddress = "smtp.mail.ru";
    private const string BaseMessage = "Администрация портала - Galeon";
    private const int Port = 465;

    private readonly EmailGatewayOptions _options;
    private readonly SmtpClient _smtpClient;

    public EmailGateway(IOptions<EmailGatewayOptions> options, SmtpClient smtpClient)
    {
        _smtpClient = smtpClient;
        _options = options.Value;
    }

    public async Task SendEmail(SendEmailDto sendEmailDto, CancellationToken cancellationToken)
    {
        var emailMessage = CreateMimeMessage(sendEmailDto, _options.Email);

        await _smtpClient.ConnectAsync(MainAddress, Port, true, cancellationToken);
        await _smtpClient.AuthenticateAsync(_options.Email, _options.Password, cancellationToken);
        await _smtpClient.SendAsync(emailMessage, cancellationToken);
        await _smtpClient.DisconnectAsync(true, cancellationToken);
    }

    private static MimeMessage CreateMimeMessage(SendEmailDto sendEmailDto, string adminEmail)
    {
        var fromAddress = new MailboxAddress(BaseMessage, adminEmail);
        var toAddress = new MailboxAddress(MainAddress, sendEmailDto.Email);
        var body = new TextPart(TextFormat.Html)
        {
            Text = sendEmailDto.Message
        };

        return new ()
        {
            From = { fromAddress },
            To = { toAddress },
            Body = body,
            Subject = sendEmailDto.Subject
        };
    }
}
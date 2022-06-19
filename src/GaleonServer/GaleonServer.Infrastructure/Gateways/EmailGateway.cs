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
	private const string BaseMessage = "Администрация портала - Galeon";
	private const string MainAddress = "smtp.mail.ru";
	private const int Port = 465;

	private readonly EmailGatewayOptions _options;

	public EmailGateway(IOptions<EmailGatewayOptions> options)
	{
		_options = options.Value;
	}

	public async Task SendEmail(SendEmailDto sendEmailDto, CancellationToken cancellationToken)
	{
		var emailMessage = CreateMimeMessage(sendEmailDto, _options.Email);

		using var smtpClient = new SmtpClient();
		await smtpClient.ConnectAsync(MainAddress, Port, true, cancellationToken);
		await smtpClient.AuthenticateAsync(_options.Email, _options.Password, cancellationToken);
		await smtpClient.SendAsync(emailMessage, cancellationToken);
		await smtpClient.DisconnectAsync(true, cancellationToken);
	}

	private static MimeMessage CreateMimeMessage(SendEmailDto sendEmailDto, string adminEmail)
	{
		var fromAddress = new MailboxAddress(BaseMessage, adminEmail);
		var toAddress = new MailboxAddress(MainAddress, sendEmailDto.Email);
		var body = new TextPart(TextFormat.Html)
		{
			Text = sendEmailDto.Message
		};

		return new()
		{
			From = { fromAddress },
			To = { toAddress },
			Body = body,
			Subject = BaseMessage
		};
	}
}
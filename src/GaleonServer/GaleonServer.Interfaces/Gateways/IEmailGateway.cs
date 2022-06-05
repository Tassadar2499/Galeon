using GaleonServer.Models.Dto;

namespace GaleonServer.Interfaces.Gateways;

public interface IEmailGateway
{
    Task SendEmail(SendEmailDto sendEmailDto, CancellationToken cancellationToken);
}
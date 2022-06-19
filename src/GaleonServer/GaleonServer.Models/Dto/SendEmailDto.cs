namespace GaleonServer.Models.Dto;

public class SendEmailDto
{
    public string Email { get; init; } = null!;
    public string Message { get; init; } = null!;
}
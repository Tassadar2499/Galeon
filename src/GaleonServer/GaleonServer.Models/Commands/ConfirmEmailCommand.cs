namespace GaleonServer.Models.Commands;

public class ConfirmEmailCommand
{
    public string UserId { get; init; }
    public string Code { get; init; }
}
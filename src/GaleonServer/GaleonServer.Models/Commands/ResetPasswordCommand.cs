namespace GaleonServer.Models.Commands;

public class ResetPasswordCommand
{
    //TODO: Add fluent validation
    public string Email { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
    public string Code { get; init; }
}
namespace GaleonServer.Models.Queries;

public class LoginQuery
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}
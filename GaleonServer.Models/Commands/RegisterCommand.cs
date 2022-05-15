using MediatR;

namespace GaleonServer.Models.Commands;

public class RegisterCommand : IRequest<Unit>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PasswordConfirm { get; set; } = null!;
}
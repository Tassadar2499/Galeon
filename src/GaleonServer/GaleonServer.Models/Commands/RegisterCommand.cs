using System.Text.Json.Serialization;
using GaleonServer.Models.Dto;

namespace GaleonServer.Models.Commands;

public class RegisterCommand
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string PasswordConfirm { get; init; } = null!;

    [JsonIgnore]
    public Func<UserCallbackUrlDto, string?>? CreateCallbackUrl { get; private set; }

    public void SetCreateCallbackUrl(Func<UserCallbackUrlDto, string?> func)
    {
        CreateCallbackUrl = func;
    }
}
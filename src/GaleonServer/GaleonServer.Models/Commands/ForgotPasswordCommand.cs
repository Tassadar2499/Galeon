using System.Text.Json.Serialization;
using GaleonServer.Models.Dto;
using GaleonServer.Models.Interfaces.Requests;

namespace GaleonServer.Models.Commands;

public class ForgotPasswordCommand : IAuthorizationServiceRequest
{
    public string Email { get; init; }
    
    [JsonIgnore]
    public Func<UserCallbackUrlDto, string?>? CreateCallbackUrl { get; private set; }
    
    public void SetCreateCallbackUrl(Func<UserCallbackUrlDto, string?> func)
    {
        CreateCallbackUrl = func;
    }
}
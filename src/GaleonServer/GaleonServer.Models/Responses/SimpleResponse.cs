using GaleonServer.Models.Interfaces.Responses;

namespace GaleonServer.Models.Responses;

public class SimpleResponse
{
    public bool Succeed { get; init; }
    public string Error { get; init; }

    public static SimpleResponse CreateError(IEnumerable<string> errors)
    {
        var errorsStr = string.Join(Environment.NewLine, errors);
        
        return new()
        {
            Succeed = false,
            Error = errorsStr
        };
    }
    
    public static SimpleResponse CreateError(string error)
    {
        return new()
        {
            Succeed = false,
            Error = error
        };
    }
    
    public static SimpleResponse CreateSucceed()
    {
        return new()
        {
            Succeed = true
        };
    }
}
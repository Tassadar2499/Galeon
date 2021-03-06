namespace GaleonServer.Models.Responses;

public class SimpleResponse
{
    public bool Succeed { get; set; }
    public string Error { get; set; }

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
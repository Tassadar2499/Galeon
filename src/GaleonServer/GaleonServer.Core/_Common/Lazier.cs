using Microsoft.Extensions.DependencyInjection;

namespace GaleonServer.Core._Common;

public class Lazier<T> : Lazy<T> where T : class
{
    public Lazier(IServiceProvider provider) : base(provider.GetRequiredService<T>)
    {
    }
}
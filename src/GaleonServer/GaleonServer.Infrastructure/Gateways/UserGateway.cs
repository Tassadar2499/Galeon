using GaleonServer.Core.Models;
using GaleonServer.Infrastructure.Database;
using GaleonServer.Interfaces.Gateways;
using Microsoft.EntityFrameworkCore;

namespace GaleonServer.Infrastructure.Gateways;

public class UserGateway : IUserGateway
{
    private readonly GaleonContext _galeonContext;

    public UserGateway(GaleonContext galeonContext)
    {
        _galeonContext = galeonContext;
    }

    public async Task DeleteUserById(string id, CancellationToken cancellationToken)
    {
        var user = new User { Id = id };
        
        var usersDbSet = _galeonContext.Users;
        usersDbSet.Attach(user);
        usersDbSet.Remove(user);

        await SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _galeonContext.SaveChangesAsync(cancellationToken);
    }
}
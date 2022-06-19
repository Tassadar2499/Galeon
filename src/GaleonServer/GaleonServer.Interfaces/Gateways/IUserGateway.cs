namespace GaleonServer.Interfaces.Gateways;

public interface IUserGateway
{
    //TODO: Перенести UserManager сюда

    public Task DeleteUserById(string id, CancellationToken cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken);
}
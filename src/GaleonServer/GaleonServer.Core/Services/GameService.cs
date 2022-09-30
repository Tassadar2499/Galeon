using GaleonServer.Core.Gateways;
using GaleonServer.Core.Helpers;
using GaleonServer.Interfaces.Gateways;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Flags;
using GaleonServer.Models.Models;
using GaleonServer.Models.Responses;

namespace GaleonServer.Core.Services;

public interface IGameService
{
    IAsyncEnumerable<GameResponse> GetAll(CancellationToken cancellationToken);
    ValueTask<SimpleResponse> CreateOrUpdate(GameCommands.CreateOrUpdateCommand command, CancellationToken cancellationToken);
}

public class GameService : IGameService
{
    private readonly IGameGateway _gameGateway;
    private readonly IGameReadonlyGateway _gameReadonlyGateway;

    public GameService(IGameGateway gameGateway, IGameReadonlyGateway gameReadonlyGateway)
    {
        _gameGateway = gameGateway;
        _gameReadonlyGateway = gameReadonlyGateway;
    }

    public IAsyncEnumerable<GameResponse> GetAll(CancellationToken cancellationToken)
    {
        return _gameReadonlyGateway.GetAll(cancellationToken);
    }

    public async ValueTask<SimpleResponse> CreateOrUpdate(GameCommands.CreateOrUpdateCommand command, CancellationToken cancellationToken)
    {
        Func<GameCommands.CreateOrUpdateCommand, CancellationToken, ValueTask<SimpleResponse>> action =
            command.Id.HasValue
                ? Update
                : Create;

        return await action(command, cancellationToken);
    }
    
    private async ValueTask<SimpleResponse> Create(GameCommands.CreateOrUpdateCommand command, CancellationToken cancellationToken)
    {
        var game = new Game();
        game.InitData(command);

        await _gameGateway.Create(game, cancellationToken);
        
        return SimpleResponse.CreateSucceed();
    }
    
    private async ValueTask<SimpleResponse> Update(GameCommands.CreateOrUpdateCommand command, CancellationToken cancellationToken)
    {
        var game = await _gameGateway.Get(command.Id!.Value, GameFetchOptions.MagnetLinks, cancellationToken)
                   ?? throw new Exception();
            
        game.InitData(command);

        await _gameGateway.SaveChanges(cancellationToken);
        
        return SimpleResponse.CreateSucceed();
    }
}
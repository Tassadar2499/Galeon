using GaleonServer.Core.Gateways;
using GaleonServer.Core.Services.Interfaces;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Responses;
using MediatR;

namespace GaleonServer.Core.Services;

public class GameService : IGameService
{
    private readonly Lazy<IGameReadonlyGateway> _gameReadonlyGateway;
    private readonly Lazy<IGameGateway> _gameGateway;

    public GameService(Lazy<IGameReadonlyGateway> gameReadonlyGateway, Lazy<IGameGateway> gameGateway)
    {
        _gameReadonlyGateway = gameReadonlyGateway;
        _gameGateway = gameGateway;
    }

    public async Task<TResult> Handle<TRequest, TResult>(TRequest request, CancellationToken cancellationToken)
    {
        object response = request switch
        {
            AddGameCommand addGameCommand => await HandleAddGame(addGameCommand, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(request), request, null)
        };

        return (TResult)response;
    }

    public IAsyncEnumerable<TResult> HandleEnumerable<TRequest, TResult>(TRequest request, CancellationToken cancellationToken)
    {
        object response = request switch
        {
            Unit handleGetAllQuery => HandleGetAll(handleGetAllQuery, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(request), request, null)
        };

        return (IAsyncEnumerable<TResult>) response;
    }

    private IAsyncEnumerable<GameResponse> HandleGetAll(Unit _, CancellationToken cancellationToken)
    {
        return _gameReadonlyGateway.Value.GetAll(cancellationToken);
    }

    private async Task<SimpleResponse> HandleAddGame(AddGameCommand command, CancellationToken cancellationToken)
    {
        await _gameGateway.Value.Add(command.Name, cancellationToken);

        return SimpleResponse.CreateSucceed();
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using GaleonServer.Core.Services.Interfaces;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Queries;
using GaleonServer.Models.Responses;

namespace GaleonServer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
	private readonly IGameService _gameService;

	public GameController(IGameService gameService)
	{
		_gameService = gameService;
	}

	[HttpGet("all")]
	public async IAsyncEnumerable<GameResponse> GetAll([EnumeratorCancellation] CancellationToken cancellationToken)
	{
		var request = new Unit();

		var games = _gameService.HandleEnumerable<Unit, GameResponse>(request, cancellationToken);

		await foreach (var game in games.WithCancellation(cancellationToken))
			yield return game;
	}

	[HttpPost("add")]
	public async Task<SimpleResponse> Add([FromBody] AddGameCommand command, CancellationToken cancellationToken)
	{
		return await _gameService.Handle<AddGameCommand, SimpleResponse>(command, cancellationToken);
	}
}
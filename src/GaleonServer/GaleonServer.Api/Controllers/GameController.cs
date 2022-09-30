using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using GaleonServer.Core.Services;
using GaleonServer.Models.Commands;
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
		var games = _gameService.GetAll(cancellationToken);

		await foreach (var game in games.WithCancellation(cancellationToken))
			yield return game;
	}

	[HttpPost("create-or-update")]
	public async Task<SimpleResponse> CreateOrUpdate(GameCommands.CreateOrUpdateCommand command, CancellationToken cancellationToken)
	{
		return await _gameService.CreateOrUpdate(command, cancellationToken);
	}
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Queries;
using GaleonServer.Models.Responses;

namespace GaleonServer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
	private readonly IMediator _mediator;

	public GameController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("all")]
	public async IAsyncEnumerable<GameResponse> GetAll([EnumeratorCancellation] CancellationToken cancellationToken)
	{
		var query = new GetAllGamesQuery();

		var games = _mediator.CreateStream(query, cancellationToken);

		await foreach (var game in games.WithCancellation(cancellationToken))
			yield return game;
	}

	[HttpPost("add")]
	public async Task Add([FromBody] AddGameCommand command, CancellationToken cancellationToken)
	{
		_ = await _mediator.Send(command, cancellationToken);
	}
}
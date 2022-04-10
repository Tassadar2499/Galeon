using GaleonServer.Core.Commands;
using GaleonServer.Core.Dto;
using GaleonServer.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GaleonServer.Api.Controllers
{
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
		public async Task<IReadOnlyCollection<GameDto>> GetAll(CancellationToken cancellationToken)
		{
			var query = new GetAllGamesQuery();

			return await _mediator.Send(query, cancellationToken);
		}

		[HttpPost("add")]
		public async Task Add([FromBody] AddGameCommand command, CancellationToken cancellationToken)
		{
			_ = await _mediator.Send(command, cancellationToken);
		}
	}
}
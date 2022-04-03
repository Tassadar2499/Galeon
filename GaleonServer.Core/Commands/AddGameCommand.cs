using GaleonServer.Core.Gateways;
using MediatR;

namespace GaleonServer.Core.Commands
{
	public class AddGameCommand : IRequest
	{
		public string Name { get; set; }
	}

	public class AddGameCommandHandler : IRequestHandler<AddGameCommand>
	{
		private readonly IGameGateway _gameGateway;

		public AddGameCommandHandler(IGameGateway gameGateway)
		{
			_gameGateway = gameGateway;
		}

		public async Task<Unit> Handle(AddGameCommand request, CancellationToken cancellationToken)
		{
			await _gameGateway.Add(request.Name);

			return new Unit();
		}
	}
}
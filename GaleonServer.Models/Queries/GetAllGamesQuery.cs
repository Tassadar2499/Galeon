using GaleonServer.Models.Responses;
using MediatR;

namespace GaleonServer.Models.Queries;

public class GetAllGamesQuery : IStreamRequest<GameResponse>
{
}
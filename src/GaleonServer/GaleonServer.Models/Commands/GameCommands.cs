namespace GaleonServer.Models.Commands;

public static class GameCommands
{
    public class CreateOrUpdateCommand
    {
        public int? Id { get; init; }
        public string Name { get; init; } = null!;
        public ICollection<MagnetLinkDto> MagnetLinks { get; init; } = Array.Empty<MagnetLinkDto>();

        public class MagnetLinkDto
        {
            public int? Id { get; init; }
            public string Value { get; init; } = null!;
        }
    }
}
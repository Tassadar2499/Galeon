using GaleonServer.Core._Common;
using GaleonServer.Core.Models;
using GaleonServer.Models.Commands;
using GaleonServer.Models.Models;

namespace GaleonServer.Core.Helpers;

public static class GameInitializeHelper
{
    public static void InitData(this Game game, GameCommands.CreateOrUpdateCommand command)
    {
        game.Name = command.Name;
        command.MagnetLinks.ForEach(game.InitMagnetLink);
    }

    private static void InitMagnetLink(this Game game, GameCommands.CreateOrUpdateCommand.MagnetLinkDto magnetLinkDto)
    {
        MagnetLink? magnetLink;
			
        if (magnetLinkDto.Id.HasValue)
        {
            magnetLink = game.MagnetLinks.FirstOrDefault(z => z.Id == magnetLinkDto.Id) ?? throw new Exception();
        }
        else
        {
            magnetLink = new MagnetLink();
            game.MagnetLinks.Add(magnetLink);
        }

        magnetLink.Value = magnetLinkDto.Value;
    }
}
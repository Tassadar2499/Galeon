using MonoTorrent;
using MonoTorrent.Client;

namespace TorrentTest
{
	public static class ExecutorV2
	{
		public static async Task ExecuteAsync()
		{
			const string magnetLink = "magnet:?xt=urn:btih:A2CAEC10220C62B1F963C7F4C647B60279EC7D7E&tr=http%3A%2F%2Fbt4.t-ru.org%2Fann%3Fmagnet&dn=Broforce%20%5BL%5D%20%5BENG%20%2B%207%20%2F%20ENG%5D%20(2015)%20(1130)%20%5BGOG%5D";
			const string saveDirectory = "";

			var settingBuilder = new EngineSettingsBuilder
			{
				AllowPortForwarding = true,
				AutoSaveLoadDhtCache = true,
				AutoSaveLoadFastResume = true,
				AutoSaveLoadMagnetLinkMetadata = true,
				ListenPort = 55123,
				DhtPort = 55123,
				FastResumeMode = FastResumeMode.BestEffort
			};

			var engineSetting = settingBuilder.ToSettings();

			using var engine = new ClientEngine(settingBuilder.ToSettings());
			var magnet = MagnetLink.Parse(magnetLink);

			var torrentSettingBuilder = new TorrentSettingsBuilder
			{
				AllowInitialSeeding = true,
				UploadSlots = 0
			};

			var manager = await engine.AddAsync(magnet, saveDirectory, torrentSettingBuilder.ToSettings());
			await engine.StartAllAsync();
			while (manager.State is not (TorrentState.Stopped or TorrentState.Seeding))
			{
				if (manager.State == TorrentState.Downloading)
				{
					Console.Clear();
					var percent = Math.Round(manager.Progress, 2);
					Console.WriteLine($"Progress: {percent}");
				}
				await Task.Delay(1000);
			}
		}
	}
}
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
				// Allow the engine to automatically forward ports using upnp/nat-pmp (if a compatible router is available)
				AllowPortForwarding = true,

				// Automatically save a cache of the DHT table when all torrents are stopped.
				AutoSaveLoadDhtCache = true,

				// Automatically save 'FastResume' data when TorrentManager.StopAsync is invoked, automatically load it
				// before hash checking the torrent. Fast Resume data will be loaded as part of 'engine.AddAsync' if
				// torrent metadata is available. Otherwise, if a magnetlink is used to download a torrent, fast resume
				// data will be loaded after the metadata has been downloaded. 
				AutoSaveLoadFastResume = true,

				// If a MagnetLink is used to download a torrent, the engine will try to load a copy of the metadata
				// it's cache directory. Otherwise the metadata will be downloaded and stored in the cache directory
				// so it can be reloaded later.
				AutoSaveLoadMagnetLinkMetadata = true,

				// Use a fixed port to accept incoming connections from other peers.
				ListenPort = 55123,

				// Use a random port for DHT communications.
				DhtPort = 55123,
			};

			using var engine = new ClientEngine(settingBuilder.ToSettings());
			var magnet = MagnetLink.Parse(magnetLink);
			var manager = await engine.AddAsync(magnet, saveDirectory);
			await engine.StartAllAsync();
			while (manager.State != TorrentState.Stopped)
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

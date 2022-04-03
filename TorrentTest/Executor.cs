using Newtonsoft.Json;
using SuRGeoNix.BitSwarmLib;

namespace TorrentTest
{
	public class Executor
	{
		private readonly BitSwarm _bitSwarm;
		private bool _sessionFinished;

		public Executor()
		{
			_bitSwarm = new BitSwarm();
			_sessionFinished = false;
		}

		public async Task Execute()
		{
			//Костыль чтобы удалить файлы из temp, потом перепилим хуле
			var directoryInfo = new DirectoryInfo(_bitSwarm.Options.FolderSessions);
			directoryInfo = directoryInfo.Parent;
			var subDirectories = Directory.GetDirectories(directoryInfo.FullName).ToList();
			subDirectories.ForEach(z => Directory.Delete(z, true));

			var settingText = File.ReadAllText("Setting.json");
			var setting = JsonConvert.DeserializeObject<Setting>(settingText)!;
			var magnetLink = setting.MagnetLink;

			_bitSwarm.Options.FolderComplete = string.IsNullOrEmpty(setting.CompleteFolder)
				? Directory.GetCurrentDirectory()
				: setting.CompleteFolder;

			//var files = Directory.GetFiles("test");
			//var filePath = Array.Find(files, z => new FileInfo(z).Extension == ".torrent");

			// Step 2: Subscribe events

			// Receives torrent data (on torrent file/session will fire directly, on magnetlink/hash will fire on metadata received - notify user with torrent detail and optionally choose which files to include)
			_bitSwarm.MetadataReceived += BitSwarm_MetadataReceived;     // e.Torrent
																		 // Receives statistics (refresh every 2 seconds - notify user with the current connections/bytes/speed of downloading)
			_bitSwarm.StatsUpdated += BitSwarm_StatsUpdated;     // e.Stats
																 // Notifies with the new status (notify user with 0: Finished, 1: Stopped, 2: Error)
			_bitSwarm.StatusChanged += BitSwarm_StatusChanged;       // e.Status
																	 // Notifies that is going to stop (user can prevent it from finishing, by including other previously excluded files)
			_bitSwarm.OnFinishing += BitSwarm_OnFinishing;       // e.Cancel

			_bitSwarm.Open(magnetLink);

			_bitSwarm.Start();

			while (!_sessionFinished)
			{
				await Task.Delay(1000);
			}

			_bitSwarm?.Dispose();
		}

		private void BitSwarm_OnFinishing(object source, BitSwarm.FinishingArgs e)
		{
			//throw new NotImplementedException();
		}

		private void BitSwarm_StatusChanged(object source, BitSwarm.StatusChangedArgs e)
		{
			if (_bitSwarm != null && _bitSwarm.isPaused && e.Status == 1)
			{
				Console.WriteLine("Paused");
				return;
			}

			if (e.Status == 0)
			{
				Console.WriteLine("\r\nDownload success!\r\n\r\n");
				Console.WriteLine(_bitSwarm?.DumpTorrent());
			}
			else if (e.Status == 2)
				Console.WriteLine("An error has been occured :( \r\n" + e.ErrorMsg);

			_bitSwarm?.Dispose();
			_sessionFinished = true;
		}

		private void BitSwarm_StatsUpdated(object source, BitSwarm.StatsUpdatedArgs e)
		{
			Console.Clear();
			Console.WriteLine(_bitSwarm?.DumpStats());
			//throw new NotImplementedException();
		}

		private void BitSwarm_MetadataReceived(object source, BitSwarm.MetadataReceivedArgs e)
		{
			Console.Clear();
			Console.WriteLine(_bitSwarm?.DumpStats());
			//throw new NotImplementedException();
		}
	}
}
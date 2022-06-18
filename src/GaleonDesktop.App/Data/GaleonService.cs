using System.Text.Json;

namespace GaleonDesktop.App.Data
{
	public class GaleonService
	{
		private static readonly string GaleonUrl = "https://localhost:62491/api/Game/all";

		public async Task<Game[]> GetAllGamesAsync()
		{
			try
			{
				using var client = new HttpClient();
				var responce = await client.GetByteArrayAsync(GaleonUrl);

				return JsonSerializer.Deserialize<Game[]>(responce);
			}
			catch
			{
				return Array.Empty<Game>();
			}
		}
	}
}
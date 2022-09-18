using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Web;
using BencodeNET.Objects;
using BencodeNET.Parsing;

//TODO
//1. Bencode parser
//2. Version BitTorrent v2
//3. Random Byte

namespace Torrent
{
	public class Program
	{
		private const int Port = 6889;
		private const string TorrentFile = "torrent/ubuntu-22.04.1-desktop-amd64.iso.torrent";

		public static async Task Main()
		{
			//parsing
			var random = new Random();
			var parser = new BencodeParser();
			await using var fileStream = File.OpenRead(TorrentFile);
			var obj = await parser.ParseAsync<BDictionary>(fileStream);

			var url = obj.Get<BString>("announce").ToString();
			var infoHash = SHA1.HashData(obj.Get<BObject>("info").EncodeAsBytes());
			var peerId = GenerateId(random);
			var left = obj.Get<BDictionary>("info").Get<BNumber>("length").Value;

			//Получаем пиры
			Console.Write($"Trying to connect to tracker - {url}");
			var peersRaw = await GetPeersAsync(parser, url, infoHash, peerId, left, Port);
			Console.WriteLine(" ... Done");

			var address = new IPAddress((peersRaw[3] << 24) | (peersRaw[2] << 16) | (peersRaw[1] << 8) | peersRaw[0]);
			var port = (peersRaw[5] << 8) | peersRaw[4];			
			var endpoint = new IPEndPoint(address, port);

			Console.Write($"Trying to connect to peer {endpoint}");
			var s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			await s.ConnectAsync(endpoint);
			Console.WriteLine("... Done");


			//Мужское Рукопожатие
			const string pstr = "BitTorrent protocol";
			var bytes = Encoding.UTF8.GetBytes(pstr);
			var array = new byte[49 + pstr.Length];
			array[0] = 19;
			bytes.CopyTo(array, 1);
			Array.Fill<byte>(array, 0, 20, 8);
			infoHash.CopyTo(array, 28);
			Encoding.UTF8.GetBytes(peerId).CopyTo(array, 48);

			Console.Write("Trying send handshake");
			s.Send(array);
			Console.WriteLine(" ... Done");


			//Console.Write("Trying recieve message");
			//var buffer = new byte[1024 * 256];
			//var result = s.Receive(buffer, SocketFlags.Truncated);
			//Console.WriteLine(" ... Done");
			//
			//Console.WriteLine(Encoding.UTF8.GetString(buffer));
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct PeerInfo
		{
			[FieldOffset(0)] public uint le_Ip;
			[FieldOffset(4)] public ushort le_Port;
			
			[FieldOffset(0)] public ushort be_Port;
			[FieldOffset(2)] public uint be_Ip;
		}

		private static async Task<byte[]> GetPeersAsync(BencodeParser parser, string url, byte[] infoHash, string peerId, long left, long port)
		{
			// info_hash — SHA1 - хеш словаря с информацией в торрент - файле;
			// peer_id — уникальный ID, сгенерированный для данного клиента;
			// uploaded — общее количество отправленных байтов;
			// downloaded — общее количество загруженных байтов;
			// left — количество байтов, которое клиенту осталось загрузить;
			// port — TCP - порт, на котором клиент слушает;
			// compact — принимает ли клиент компактный список пиров.
			var request = $"{url}?info_hash={HttpUtility.UrlEncode(infoHash)}&peer_id={peerId}&uploaded=0&downloaded=0&left={left}&port={port}&compact=1";
			using var httpClient = new HttpClient();
			return await httpClient.GetByteArrayAsync(request);
		}

		public static ReadOnlySpan<PeerInfo> ParsePeersString(ReadOnlySpan<byte> peersRaw)
		{
			if (BitConverter.IsLittleEndian)
				return MemoryMarshal.Cast<byte, PeerInfo>(peersRaw);

			var bytes = new byte[peersRaw.Length];
			peersRaw.CopyTo(bytes);
			bytes.AsSpan().Reverse();

			return MemoryMarshal.Cast<byte, PeerInfo>(bytes);
		}

		private static string GenerateId(Random random)
		{
			const int IdLength = 20;
			const int VersionLength = 6;

			Span<char> charArray = stackalloc char[IdLength] { 'Z', 'Z', '0', '0', '0', '-', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0' };

			random.NextBytes(MemoryMarshal.Cast<char, byte>(charArray[VersionLength..]));

			for (int i = VersionLength; i < IdLength; i++)
				charArray[i] = (char)(charArray[i] % 10 + 48);

			return charArray.ToString();
		}
	}
}
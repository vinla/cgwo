using System;
using System.IO;
using Cogs.Common;

namespace Cogs.Data.LiteDb
{
	public class ImageStore : IImageStore
	{
		private string _connectionString;

		public ImageStore(string connectionString)
		{
			_connectionString = connectionString;
		}

		public string ImportFromFile(string path)
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				var id = Guid.NewGuid().ToString();
				dataStore.FileStorage.Upload(id, path);
				return id;
			}
		}

		public byte[] Retrieve(string imageKey)
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				using (var stream = new MemoryStream())
				{
					dataStore.FileStorage.Download(imageKey, stream);
					return stream.GetBuffer();
				}					
			}
		}

		public void Save(ImageData imageData)
		{
			Save(imageData.Key, imageData.Data);
		}

		public void Save(string imageKey, byte[] imageData)
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				dataStore.FileStorage.Delete(imageKey);
				using (var memoryStream = new MemoryStream(imageData))
				{
					dataStore.FileStorage.Upload(imageKey, imageKey, memoryStream);
				}
			}
		}
	}

	public static class ImageIdGenerator
	{
		public static string LayoutBackground(Guid cardTypeId) => $"LBG_{cardTypeId}";

		public static string LayoutElement(Guid elementId) => $"IMGEL_{elementId}";
	}
}

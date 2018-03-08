using System;
using System.IO;

namespace Cogs.Common
{
    public interface IImageStore
    {
		string ImportFromFile(string path);
		byte[] Retrieve(string imageKey);
		void Save(string imageKey, byte[] data);
		void Save(Common.ImageData imageData);
	}
}

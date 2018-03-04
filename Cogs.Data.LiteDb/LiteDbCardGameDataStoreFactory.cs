using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cogs.Common;

namespace Cogs.Data.LiteDb
{
	public class LiteDbCardGameDataStoreFactory : ICardGameDataStoreFactory
	{
		public ICardGameDataStore Create(Dictionary<string, string> parameters)
		{
			if (parameters.ContainsKey("FilePath") == false)
				throw new ArgumentException("FilePath is required");

			var filePath = parameters["FilePath"];

			if (System.IO.File.Exists(filePath))
				throw new InvalidOperationException("File already exists");

			return new LiteDbCardGameDataStore(filePath);
		}

		public ICardGameDataStore Open(Dictionary<string, string> parameters)
		{
			if (parameters.ContainsKey("FilePath") == false)
				throw new ArgumentException("FilePath is required");

			var filePath = parameters["FilePath"];

			if (System.IO.File.Exists(filePath) == false)
				throw new InvalidOperationException("File does not exist");

			return new LiteDbCardGameDataStore(filePath);
		}
	}
}

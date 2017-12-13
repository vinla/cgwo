using System;
using System.Collections.Generic;
using Cogs.Common;

namespace Cogs.Core
{
    public class JsonCardGameDataStoreFactory : ICardGameDataStoreFactory
    {
        public ICardGameDataStore Create(Dictionary<string, string> parameters)
        {
            if (parameters.ContainsKey("FilePath") == false)
                throw new ArgumentException("FilePath is required");

            var filePath = parameters["FilePath"];

            return JsonCardGameDataStore.CreateNew(filePath);
        }

        public ICardGameDataStore Open(Dictionary<string, string> parameters)
        {
            if (parameters.ContainsKey("FilePath") == false)
                throw new ArgumentException("FilePath is required");

            var filePath = parameters["FilePath"];

            return JsonCardGameDataStore.OpenFile(filePath);            
        }
    }
}

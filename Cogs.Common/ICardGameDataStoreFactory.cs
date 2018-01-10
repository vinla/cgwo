using System.Collections.Generic;

namespace Cogs.Common
{
    public interface ICardGameDataStoreFactory
    {
        ICardGameDataStore Open(Dictionary<string, string> parameters);
        ICardGameDataStore Create(Dictionary<string, string> parameters);
    }
}

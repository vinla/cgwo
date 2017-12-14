using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogs.Common
{
    public interface ICardGameDataStore
    {
        ProjectInfo GetProjectInfo();
        void SetProjectInfo(ProjectInfo projectInfo);
        IEnumerable<CardType> GetCardTypes();
        void SaveCardType(CardType cardType);
        void DeleteCardType(CardType cardType);
    }    

    public interface ICardGameDataStoreFactory
    {
        ICardGameDataStore Open(Dictionary<string, string> parameters);
        ICardGameDataStore Create(Dictionary<string, string> parameters);
    }
}

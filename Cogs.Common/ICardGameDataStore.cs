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
        IEnumerable<CardAttribute> GetCardAttributes(Guid cardTypeId);
        void SaveCardAttribute(Guid cardTypeId, CardAttribute attribute);
        void DeleteCardAttribute(CardAttribute attribute);
        CardLayout GetLayout(Guid cardTypeId);
        void SaveLayout(Guid cardTypeId, CardLayout layout);
        void SaveCard(Card card);
        IEnumerable<Card> GetCards();
		void DeleteCard(Card card);
        void UpdateCardTypeImage(Guid cardTypeId, byte[] imageData);
	}  
}

using System;
using System.Collections.Generic;
using Cogs.Common;
using LiteDB;

namespace Cogs.Data.LiteDb
{
	public class NoSqlCardGameDataStore : ICardGameDataStore
	{
		public void DeleteCard(Card card)
		{
			using (var db = new LiteDatabase("database.db"))
			{
				db.GetCollection(nameof(Card)).Delete(card.Id);
			}
		}

		public void DeleteCardAttribute(CardAttribute attribute)
		{
			using (var db = new LiteDatabase("database.db"))
			{
				
			}
		}

		public void DeleteCardType(CardType cardType)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<CardAttribute> GetCardAttributes(Guid cardTypeId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Card> GetCards()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<CardType> GetCardTypes()
		{
			throw new NotImplementedException();
		}

		public CardLayout GetLayout(Guid cardTypeId)
		{
			throw new NotImplementedException();
		}

		public ProjectInfo GetProjectInfo()
		{
			throw new NotImplementedException();
		}

		public void SaveCard(Card card)
		{
			throw new NotImplementedException();
		}

		public void SaveCardAttribute(Guid cardTypeId, CardAttribute attribute)
		{
			throw new NotImplementedException();
		}

		public void SaveCardType(CardType cardType)
		{
			throw new NotImplementedException();
		}

		public void SaveLayout(Guid cardTypeId, CardLayout layout)
		{
			throw new NotImplementedException();
		}

		public void SetProjectInfo(ProjectInfo projectInfo)
		{
			throw new NotImplementedException();
		}

		public void UpdateCardTypeImage(Guid cardTypeId, byte[] imageData)
		{
			throw new NotImplementedException();
		}
	}
}

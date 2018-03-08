using System;
using System.Collections.Generic;
using System.Linq;
using Cogs.Common;
using AutoMapper;

namespace Cogs.Data.LiteDb
{
	public class LiteDbCardGameDataStore : ICardGameDataStore
	{
		private readonly string _connectionString;

		public LiteDbCardGameDataStore(string filePath)
		{
			_connectionString = filePath;			
		}

		public void DeleteCard(Card card)
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				dataStore.GetCollection<Json.Card>().Delete(card.Id);
				// TODO: Need to delete the image for this card
			}
		}

		public void DeleteCardAttribute(CardAttribute attribute)
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				var cardType = dataStore.GetCollection<Json.CardType>().FindAll().SingleOrDefault(ct => ct.Attributes.Any(attr => attr.Id == attribute.Id));
				if (cardType == null)
					throw new InvalidOperationException("Card type does not exist");
				var attributes = cardType.Attributes.Where(attr => attr.Id != attribute.Id).ToList();				
				cardType.Attributes = attributes.ToArray();
				dataStore.GetCollection<Json.CardType>().Update(cardType);

				// TODO: Need to go through all existing cards of that type and remove the card attribute value
			}
		}

		public void DeleteCardType(CardType cardType)
		{
			throw new NotImplementedException();			
		}

		public IEnumerable<CardAttribute> GetCardAttributes(Guid cardTypeId)
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				var card = dataStore.GetCollection<Json.CardType>().FindById(cardTypeId);
				if (card != null)
					return card.Attributes.Select(Mapper.Map<CardAttribute>);
				else
					return Enumerable.Empty<CardAttribute>();
			}
		}

		public IEnumerable<Card> GetCards()
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				return dataStore.GetCollection<Json.Card>().FindAll().Select(Mapper.Map<Card>);
				// TODO: Need to get the images for the cards (maybe we should be doing this separately and async)
			}
		}

		public IEnumerable<CardType> GetCardTypes()
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				return dataStore.GetCollection<Json.CardType>().FindAll().Select(Mapper.Map<CardType>);
			}
		}

		public CardLayout GetLayout(Guid cardTypeId)
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				var jsonLayout = dataStore.GetCollection<Json.CardLayout>().FindById(cardTypeId);
				return jsonLayout != null ? Mapper.Map<CardLayout>(jsonLayout) : null;
			}
		}

		public ProjectInfo GetProjectInfo()
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				return dataStore.GetCollection<ProjectInfo>().FindAll().First();
			}
		}

		public void SaveCard(Card card)
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				var jsonCard = Mapper.Map<Json.Card>(card);
				var cardStore = dataStore.GetCollection<Json.Card>();
				cardStore.Delete(card.Id);
				cardStore.Insert(jsonCard);								
			}
		}

		public void SaveCardAttribute(Guid cardTypeId, CardAttribute attribute)
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				var cardType = dataStore.GetCollection<Json.CardType>().FindById(cardTypeId);
				if (cardType == null)
					throw new InvalidOperationException("Card type does not exist");
				var attributes = cardType.Attributes.Where(attr => attr.Id != attribute.Id).ToList();
				attributes.Add(Mapper.Map <Json.CardAttribute>(attribute));
				cardType.Attributes = attributes.ToArray();
				dataStore.GetCollection<Json.CardType>().Update(cardType);
			}
		}

		public void SaveCardType(CardType cardType)
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				var jsonCardType = Mapper.Map<Json.CardType>(cardType);
				var cardTypeStore = dataStore.GetCollection<Json.CardType>();
				cardTypeStore.Delete(cardType.Id);
				cardTypeStore.Insert(jsonCardType);
			}
		}

		public void SaveLayout(Guid cardTypeId, CardLayout layout)
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				var jsonLayout = Mapper.Map<Json.CardLayout>(layout);
				jsonLayout.Id = cardTypeId;

				var cardLayoutStore = dataStore.GetCollection<Json.CardLayout>();
				cardLayoutStore.Delete(jsonLayout.Id);
				cardLayoutStore.Insert(jsonLayout);
			}
		}

		public void SetProjectInfo(ProjectInfo projectInfo)
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				dataStore.GetCollection<ProjectInfo>().Insert(projectInfo);
			}
		}

		public void UpdateCardTypeImage(Guid cardTypeId, byte[] imageData)
		{
			SaveImage("CT_" + cardTypeId, imageData);
		}

		private void SaveImage(string imageKey, byte[] imageData)
		{
			using (var dataStore = new LiteDB.LiteDatabase(_connectionString))
			{
				dataStore.FileStorage.Delete(imageKey);
				using (var dataStream = dataStore.FileStorage.OpenWrite(imageKey, imageKey))
				{
					using (var writer = new System.IO.StreamWriter(dataStream))
					{
						writer.Write(imageData);
					}
				}
			}
		}
	}
}

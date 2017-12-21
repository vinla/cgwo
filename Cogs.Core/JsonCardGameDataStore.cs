using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Cogs.Common;
using Newtonsoft.Json;

namespace Cogs.Core
{
    public class JsonCardGameDataStore : ICardGameDataStore
    {
        private readonly String _fileLocation;
        private readonly JsonCardGameData _cardGameData;

        public static ICardGameDataStore OpenFile(string fileLocation)
        {
            if (File.Exists(fileLocation) == false)
                throw new FileNotFoundException("Could not open data files");

            var gameData = JsonConvert.DeserializeObject<JsonCardGameData>(File.ReadAllText(fileLocation));
            return new JsonCardGameDataStore(fileLocation, gameData);
        }

        public static ICardGameDataStore CreateNew(string fileLocation)
        {
            if (File.Exists(fileLocation))
                throw new InvalidOperationException("File already exists");

            var gameData = new JsonCardGameData();
            var store = new JsonCardGameDataStore(fileLocation, gameData);
            store.SaveChanges();
            return store;
        }

        private JsonCardGameDataStore(string fileLocation, JsonCardGameData data)
        {
            _fileLocation = fileLocation;
            _cardGameData = data;
        }

        public ProjectInfo GetProjectInfo()
        {
            return new ProjectInfo
            {
                Name = _cardGameData.ProjectInfo.Name
            };
        }

        public void SetProjectInfo(ProjectInfo projectInfo)
        {
            _cardGameData.ProjectInfo.Name = projectInfo.Name;
            SaveChanges();
        }

        public IEnumerable<CardType> GetCardTypes()
        {
            return
                _cardGameData.CardTypes.Select(ct => new CardType
                {
                    Id = ct.Id,
                    Name = ct.Name
                });
        }

        public void SaveCardType(CardType cardType)
        {
			var cardTypeData = _cardGameData.CardTypes.SingleOrDefault(ct => ct.Id == cardType.Id);

			if(cardTypeData == null)
			{
                cardTypeData = new JsonCardType
                {
                    Id = cardType.Id
                };
                _cardGameData.CardTypes.Add(cardTypeData);				
			}

            cardTypeData.Name = cardType.Name;
			SaveChanges();
        }

        public void DeleteCardType(CardType cardType)
        {
            _cardGameData.CardTypes.RemoveAll(ct => ct.Id == cardType.Id);
            _cardGameData.CardAttributes.RemoveAll(ca => ca.CardTypeId == cardType.Id);
            SaveChanges();
        }

        public IEnumerable<CardAttribute> GetCardAttributes(Guid cardTypeId)
        {
            return
                _cardGameData.CardAttributes
                    .Where(ca => ca.CardTypeId == cardTypeId)
                    .Select(ca => new CardAttribute
                    {
                        Id = ca.Id,
                        Name = ca.Name
                    });
        }

        public void SaveCardAttribute(Guid cardTypeId, CardAttribute attribute)
        {
            var attributeData = _cardGameData.CardAttributes.SingleOrDefault(ca => ca.Id == attribute.Id);

            if(attributeData == null)
            {
                attributeData = new JsonCardAttribute
                {
                    Id = attribute.Id,
                    CardTypeId = cardTypeId
                };
                _cardGameData.CardAttributes.Add(attributeData);
            }

            if (attributeData.CardTypeId != cardTypeId)
                throw new InvalidOperationException("Mismatch between card type ids");

            attributeData.Name = attribute.Name;
            SaveChanges();
        }

        public void DeleteCardAttribute(CardAttribute attribute)
        {
            _cardGameData.CardAttributes.RemoveAll(ca => ca.Id == attribute.Id);
            SaveChanges();
        }

        public CardLayout GetLayout(Guid cardTypeId)
        {
            var jsonLayout = _cardGameData.CardLayouts.SingleOrDefault(l => l.CardTypeId == cardTypeId);
            if (jsonLayout != null)
                return jsonLayout.ToLayout();
            return null;
        }

        public void SaveLayout(Guid cardTypeId, CardLayout cardLayout)
        {
            _cardGameData.CardLayouts.RemoveAll(l => l.CardTypeId == cardTypeId);
            var layoutData = JsonCardLayout.FromLayout(cardLayout);
            layoutData.CardTypeId = cardTypeId;
            _cardGameData.CardLayouts.Add(layoutData);
            SaveChanges();
        }

        private void SaveChanges()
        {
            lock(_cardGameData)
            {
                File.WriteAllText(_fileLocation, JsonConvert.SerializeObject(_cardGameData));
            }
        }
    }
}

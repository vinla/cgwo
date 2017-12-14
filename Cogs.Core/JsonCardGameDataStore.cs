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
			var existingCardType = _cardGameData.CardTypes.SingleOrDefault(ct => ct.Id == cardType.Id);
			if(existingCardType != null)
			{
				existingCardType.Name = cardType.Name;
			}
			else
			{
				_cardGameData.CardTypes.Add(new JsonCardType
				{
					Id = cardType.Id,
					Name = cardType.Name
				});
			}

			SaveChanges();
        }

        public void DeleteCardType(CardType cardType)
        {
            _cardGameData.CardTypes.RemoveAll(ct => ct.Id == cardType.Id);
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

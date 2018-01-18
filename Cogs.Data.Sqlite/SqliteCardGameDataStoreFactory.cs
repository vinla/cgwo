using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using Cogs.Common;
using EF = Cogs.Data.Sqlite.Entities;

namespace Cogs.Data.Sqlite
{
    public class SqliteCardGameDataStoreFactory : ICardGameDataStoreFactory
    {
        public ICardGameDataStore Create(Dictionary<string, string> parameters)
        {
            var path = parameters["FilePath"];
            SQLiteConnection.CreateFile(path);
            return Open(parameters);
        }

        public ICardGameDataStore Open(Dictionary<string, string> parameters)
        {
            var connectionString = $"Data Source={parameters["FilePath"]};Version=3";
            return new SqliteCardGameDataStore(connectionString);
        }
    }

    public class SqliteCardGameDataStore : ICardGameDataStore
    {
        private CardGameDataContext _dataContext;

        public SqliteCardGameDataStore(String connectionString)
        {
            _dataContext = new CardGameDataContext(connectionString);
        }

        public void DeleteCard(Card card)
        {
            _dataContext.Set<EF.Card>().RemoveWhere(c => c.Id == card.Id);
            _dataContext.Set<EF.CardAttributeValue>().RemoveWhere(ca => ca.CardId == card.Id);
            _dataContext.SaveChanges();
        }

        public void DeleteCardAttribute(CardAttribute attribute)
        {
            _dataContext.Set<EF.CardAttribute>().RemoveWhere(ca => ca.Id == attribute.Id);
            _dataContext.Set<EF.CardAttributeValue>().RemoveWhere(cav => cav.CardAttributeId == attribute.Id);
            _dataContext.SaveChanges();
        }

        public void DeleteCardType(CardType cardType)
        {
            _dataContext.Set<EF.CardType>().RemoveWhere(ct => ct.Id == cardType.Id);
            _dataContext.Set<EF.CardAttribute>().RemoveWhere(ca => ca.CardTypeId == cardType.Id);

            _dataContext.Set<EF.CardLayout>().RemoveWhere(cl => cl.CardTypeId == cardType.Id);
            _dataContext.Set<EF.CardLayoutElement>().RemoveWhere(cle => cle.CardLayout.CardTypeId == cardType.Id);

            _dataContext.Set<EF.Card>().RemoveWhere(c => c.CardTypeId == cardType.Id);
            _dataContext.Set<EF.CardAttributeValue>().RemoveWhere(cav => cav.Card.CardTypeId == cardType.Id);

            _dataContext.SaveChanges();
        }

        public IEnumerable<CardAttribute> GetCardAttributes(Guid cardTypeId)
        {
            return _dataContext.Set<EF.CardAttribute>().Where(ca => ca.CardTypeId == cardTypeId).Map<EF.CardAttribute, CardAttribute>();
        }

        public IEnumerable<Card> GetCards()
        {
            return _dataContext.Set<EF.Card>().Map<EF.Card, Card>();
        }

        public IEnumerable<CardType> GetCardTypes()
        {
            return _dataContext.Set<EF.CardType>().Map<EF.CardType, CardType>();
        }

        public CardLayout GetLayout(Guid cardTypeId)
        {
            return _dataContext.Set<EF.CardLayout>()
                .Where(cl => cl.CardTypeId == cardTypeId)
                .Map<EF.CardLayout, CardLayout>()
                .First();
        }

        public ProjectInfo GetProjectInfo()
        {
            return new ProjectInfo
            {
                Name = String.Empty
            };
        }

        public void SaveCard(Card card)
        {
            DeleteCard(card);

            var cardEntity = AutoMapper.Mapper.Map<EF.Card>(card);
            _dataContext.Set<Card>().Add(card);

            foreach (var cardValue in card.AttributeValues)
            {
                var valueEntity = AutoMapper.Mapper.Map<EF.CardAttributeValue>(cardValue);
                _dataContext.Set<EF.CardAttributeValue>().Add(valueEntity);
            }

            _dataContext.SaveChanges();
        }

        public void SaveCardAttribute(Guid cardTypeId, CardAttribute attribute)
        {
            var attributeData = _dataContext.Set<EF.CardAttribute>().FirstOrDefault(attr => attr.Id == attribute.Id);

            if (attributeData == null)
            {
                attributeData = new EF.CardAttribute
                {
                    Id = attribute.Id,
                    Type = attribute.Type,
                    CardTypeId = cardTypeId
                };
                _dataContext.Set<EF.CardAttribute>().Add(attributeData);
            }

            if (attributeData.CardTypeId != cardTypeId)
                throw new InvalidOperationException("Mismatch between card type ids");

            attributeData.Name = attribute.Name;
            _dataContext.SaveChanges();
        }

        public void SaveCardType(CardType cardType)
        {
            var cardTypeData = _dataContext.Set<EF.CardType>().FirstOrDefault(ct => ct.Id == cardType.Id);

            if (cardTypeData == null)
            {
                cardTypeData = new EF.CardType
                {
                    Id = cardType.Id
                };
                _dataContext.Set<EF.CardType>().Add(cardTypeData);
            }

            cardTypeData.Name = cardType.Name;
            _dataContext.SaveChanges();
        }

        public void SaveLayout(Guid cardTypeId, CardLayout layout)
        {
            var layoutEntity = _dataContext.Set<EF.CardLayout>().FirstOrDefault(l => l.CardTypeId == cardTypeId);

            if(layoutEntity == null)
            {
                layoutEntity = new EF.CardLayout
                {
                    Id = Guid.NewGuid(),
                    CardTypeId = cardTypeId,                    
                };
            }

            layoutEntity.BackgroundColor = layout.BackgroundColor;
            layoutEntity.BackgroundImage = Convert.FromBase64String(layout.BackgroundImage);

            _dataContext.Set<EF.CardLayoutElement>().RemoveWhere(el => el.CardLayoutId == layoutEntity.Id);

            foreach(var layoutElement in layout.Elements)
            {
                var elementEntity = new EF.CardLayoutElement
                {
                    CardLayoutId = layoutEntity.Id,
                    ElementType = layoutElement.GetType().Name,
                    JsonData = Newtonsoft.Json.JsonConvert.SerializeObject(layoutElement)
                };                
            }
        }

        public void SetProjectInfo(ProjectInfo projectInfo)
        {
            
        }
    }

    public static class DbSetExtensions
    {
        public static void RemoveWhere<T>(this DbSet<T> dbSet, Func<T, bool> predicate) where T : class
        {
            dbSet.RemoveRange(dbSet.Where(predicate));
        }

        public static IEnumerable<TDestination> Map<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            return source.Select(AutoMapper.Mapper.Map<TSource, TDestination>);
        }        
    }
}

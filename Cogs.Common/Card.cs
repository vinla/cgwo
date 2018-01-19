using System;
using System.Linq;
using System.Collections.Generic;

namespace Cogs.Common
{
    public class Card : CoreObject
    {
        private readonly CardType _cardType;
        private readonly List<CardAttributeValue> _attributeValues;
        public Card(CardType cardType, IEnumerable<CardAttributeValue> cardAttributeValues)
        {
            _cardType = cardType;
            _attributeValues = cardAttributeValues.ToList();
        }        

        public string Name { get; set; }

        public CardType CardType => _cardType;

        public IEnumerable<CardAttributeValue> AttributeValues => _attributeValues;

        public string ImageData { get; set; }
    }
}

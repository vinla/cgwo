using System;

namespace Cogs.Common
{
    public class CardAttributeValue
    {
        private readonly CardAttribute _cardAttribute;
        public CardAttributeValue(CardAttribute cardAttribute)
        {
            _cardAttribute = cardAttribute;
        }

        public CardAttribute CardAttribute => _cardAttribute;

        public string Value { get; set; }
    }
}

using System.Collections.Generic;

namespace Cogs.Common
{
    public class CardType : CoreObject
    {
        public string Name { get; set; }        

		public List<CardAttribute> Attributes { get; set; }
    }
}

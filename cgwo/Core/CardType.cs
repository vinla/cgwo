using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cgwo.Core
{
	public class CardType : CoreObject
	{
		public string Name { get; set; }			

		public CardType Clone()
		{
			var clone = new CardType();
			Imprint(clone);
			return clone;
		}

		public void Imprint(CardType target)
		{
			target.Name = Name;
		}		
	}
}

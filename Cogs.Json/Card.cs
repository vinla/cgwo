using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cogs.Json
{
	public class Card
	{
		public Guid Id { get; set; }

		public Guid CardTypeId { get; set; }

		public string Name { get; set; }

		public CardValue[] Values { get; set; }
	}
}

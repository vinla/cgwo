using System;

namespace Cogs.Json
{
	public class CardType
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public CardAttribute[] Attributes { get; set; }
	}
}

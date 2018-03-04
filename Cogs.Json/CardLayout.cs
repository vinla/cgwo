using System;

namespace Cogs.Json
{
	public class CardLayout
	{
		public Guid Id { get; set; }
		public string BackgroundColor { get; set; }
		public string BackgroundImage { get; set; }
		public ElementLayout[] Elements { get; set; }
	}
}

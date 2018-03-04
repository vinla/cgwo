using System; 

namespace Cogs.Json
{
	public class CardAttribute
	{
		public Guid Id { get; set; }
		public AttributeType Type { get; set; }
		public string Name { get; set; }
	}

	public enum AttributeType
	{
		Text,
		Image
	}
}

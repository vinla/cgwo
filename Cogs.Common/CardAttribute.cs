namespace Cogs.Common
{
    public class CardAttribute : CoreObject
    {
        public AttributeType Type { get; set; }
        public string Name { get; set; }
    }

    public enum AttributeType
    {
        Text,
        Image
    }
}

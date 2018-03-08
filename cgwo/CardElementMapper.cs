using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cogs.Common;
using Cogs.Designer;

namespace cgwo
{
    public static class CardElementMapper
    {
		public static void MapElementValues(Card card, IEnumerable<CardElement> elements)
        {
            foreach (var textElement in elements.OfType<TextElement>())
            {
                var matches = Regex.Matches(textElement.Text, "{([A-Za-z0-9]+)}");
                foreach (var match in matches.Cast<Match>().Where(m => m.Success))
                {
                    if (match.Groups[1].Value == "Name")
                        textElement.Text = textElement.Text.Replace(match.Value, card.Name);
                    else if(match.Groups[1].Value == "Type")
                        textElement.Text = textElement.Text.Replace(match.Value, card.CardType.Name);
                    else
                    {
                        var attribute = card.AttributeValues.SingleOrDefault(attr => attr.CardAttribute.Name == match.Groups[1].Value);
                        if (attribute != null)
                        {
                            textElement.Text = textElement.Text.Replace(match.Value, attribute.Value);
                        }
                    }
                }
            }

            foreach (var imageElement in elements.OfType<ImageElement>().Where(img => img.ImageSource == "Card Attribute"))
            {
                var attributeValue = card.AttributeValues.SingleOrDefault(av => av.CardAttribute.Name == imageElement.LinkedAttribute);
                if (attributeValue != null)
                {
					imageElement.ImageData = System.Convert.FromBase64String(attributeValue.Value);
                }
            }
        }
    }
}

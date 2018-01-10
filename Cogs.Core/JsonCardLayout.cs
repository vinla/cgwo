using System;
using System.Linq;
using System.Collections.Generic;
using Cogs.Common;

namespace Cogs.Core
{
    public class JsonCardLayout
    {
        public Guid CardTypeId { get; set; }

        public string BackgroundColor { get; set; }

        public string BackgroundImage { get; set; }

        public List<JsonCardElement> Elements { get; set; }

        public static JsonCardLayout FromLayout(CardLayout layout)
        {
            return new JsonCardLayout
            {
                BackgroundColor = layout.BackgroundColor,
                BackgroundImage = layout.BackgroundImage,
                Elements = layout.Elements.Select(el => new JsonCardElement
                {
                    ElementType = el.GetType().Name,
                    JsonData = Newtonsoft.Json.JsonConvert.SerializeObject(el)
                }).ToList()
            };            
        }

        public CardLayout ToLayout()
        {
            var layoutDeserializer = new ElementLayoutDeserializer();

            return new CardLayout
            {
                BackgroundColor = BackgroundColor,
                BackgroundImage = BackgroundImage,
                Elements = Elements.Select(el => layoutDeserializer.Deserialize(el)).ToList()
            };
        }
    }
}

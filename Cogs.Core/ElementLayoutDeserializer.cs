using System;
using Cogs.Common;

namespace Cogs.Core
{
    public class ElementLayoutDeserializer
    {
        public ElementLayout Deserialize(JsonCardElement json)
        {
            if (json.ElementType == typeof(TextElementLayout).Name)
                return Newtonsoft.Json.JsonConvert.DeserializeObject<TextElementLayout>(json.JsonData);
            else if (json.ElementType == typeof(RectangleElementLayout).Name)
                return Newtonsoft.Json.JsonConvert.DeserializeObject<RectangleElementLayout>(json.JsonData);
            else if (json.ElementType == typeof(EllipseElementLayout).Name)
                return Newtonsoft.Json.JsonConvert.DeserializeObject<EllipseElementLayout>(json.JsonData);
            else if (json.ElementType == typeof(ImageElementLayout).Name)
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ImageElementLayout>(json.JsonData);

            throw new InvalidOperationException("");
        }
    }
}

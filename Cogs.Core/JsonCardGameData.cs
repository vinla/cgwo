using System;
using System.Collections.Generic;
using Cogs.Common;

namespace Cogs.Core
{
    public class JsonCardGameData
    {
        public JsonCardGameData()
        {
            ProjectInfo = new JsonProjectInfo();
            CardTypes = new List<JsonCardType>();
            CardAttributes = new List<JsonCardAttribute>();
            CardLayouts = new List<JsonCardLayout>();
            Cards = new List<JsonCard>();
            CardAttributeValues = new List<JsonCardAttributeValue>();
        }

        public JsonProjectInfo ProjectInfo { get; set; }

        public List<JsonCardType> CardTypes { get; set; }

        public List<JsonCardAttribute> CardAttributes { get; set; }

        public List<JsonCardLayout> CardLayouts { get; set; }

        public List<JsonCard> Cards { get; set; }

        public List<JsonCardAttributeValue> CardAttributeValues { get; set; }
    }
}

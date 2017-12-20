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
            CardLayouts = new Dictionary<Guid, CardLayout>();
        }

        public JsonProjectInfo ProjectInfo { get; set; }

        public List<JsonCardType> CardTypes { get; set; }

        public List<JsonCardAttribute> CardAttributes { get; set; }

        public Dictionary<Guid, CardLayout> CardLayouts { get; set; }
    }
}

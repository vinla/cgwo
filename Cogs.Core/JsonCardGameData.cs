using System.Collections.Generic;

namespace Cogs.Core
{
    public class JsonCardGameData
    {       
        public JsonCardGameData()
        {
            ProjectInfo = new JsonProjectInfo();
            CardTypes = new List<JsonCardType>();
            CardAttributes = new List<JsonCardAttribute>();
        }

        public JsonProjectInfo ProjectInfo { get; set; }

        public List<JsonCardType> CardTypes { get; set; }

        public List<JsonCardAttribute> CardAttributes { get; set; }
    }
}

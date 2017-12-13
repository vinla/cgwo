using System.Collections.Generic;

namespace Cogs.Core
{
    public class JsonCardGameData
    {
        public JsonCardGameData()
        {
            ProjectInfo = new JsonProjectInfo();
            CardTypes = new List<JsonCardType>();
        }

        public JsonProjectInfo ProjectInfo { get; set; }

        public List<JsonCardType> CardTypes { get; set; }
    }
}

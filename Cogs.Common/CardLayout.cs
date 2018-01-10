using System.Collections.Generic;

namespace Cogs.Common
{
    public class CardLayout
    {
        public CardLayout()
        {
            Elements = new List<ElementLayout>();
        }

        public string BackgroundColor { get; set; }

        public string BackgroundImage { get; set; }

        public List<ElementLayout> Elements { get; set; }
    }
}

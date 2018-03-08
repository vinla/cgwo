using System;

namespace Cogs.Common
{
    public class ElementLayout
    {
		public Guid Id { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public int ZIndex { get; set; }
    }
}

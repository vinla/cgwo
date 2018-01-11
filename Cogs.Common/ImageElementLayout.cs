using System;

namespace Cogs.Common
{
    public class ImageElementLayout : ElementLayout
    {
        public string ImageSource { get; set; }
        public string ImageData { get; set; }
        public Guid LinkedAttribute { get; set; }
    }
}

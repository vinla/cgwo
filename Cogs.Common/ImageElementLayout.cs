using System;

namespace Cogs.Common
{
    public class ImageElementLayout : ElementLayout
    {
        public string ImageSource { get; set; }
        public byte[] ImageData { get; set; }
        public string LinkedAttribute { get; set; }
    }
}

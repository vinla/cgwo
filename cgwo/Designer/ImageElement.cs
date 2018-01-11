using System.Collections.Generic;
using System.Linq;

namespace Cogs.Designer
{
    public class ImageElement : CardElement
    {
        public string ImageSource
        {
            get { return GetValue<string>(nameof(ImageSource)); }
            set { SetValue(nameof(ImageSource), value); }
        }        
    }
}

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
        
        public string ImageData
        {
            get { return GetValue<string>(nameof(ImageData)); }
            set { SetValue(nameof(ImageData), value); }
        }

        public string LinkedAttribute
        {
            get { return GetValue<string>(nameof(LinkedAttribute)); }
            set { SetValue(nameof(LinkedAttribute), value); } 
        }
    }
}

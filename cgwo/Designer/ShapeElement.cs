using System.Windows.Media;

namespace Cogs.Designer
{
    public abstract class ShapeElement : CardElement
    {
        public Color BackgroundColor
        {
            get { return GetValue<Color>(nameof(BackgroundColor)); }
            set { SetValue(nameof(BackgroundColor), value); }
        }

        [cgwo.Mvvm.CalculateFrom(nameof(BackgroundColor))]
        public Brush Background => new SolidColorBrush(BackgroundColor);        

        public Color BorderColor
        {
            get { return GetValue<Color>(nameof(BorderColor)); }
            set { SetValue(nameof(BorderColor), value); }
        }

        [cgwo.Mvvm.CalculateFrom(nameof(BorderColor))]
        public Brush Border => new SolidColorBrush(BorderColor);

        public float BorderWidth
        {
            get
            {
                return GetValue<float>(nameof(BorderWidth));
            }
            set
            {
                SetValue(nameof(BorderWidth), value);
            }
        }
    }
}

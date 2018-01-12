using System.Windows;
using GorgleDevs.Wpf.Mvvm;

namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
    public class LayoutElement : ViewModel
    {
        public double Top
        {
            get { return GetValue<double>(nameof(Top)); }
            set { SetValue(nameof(Top), value); }
        }

        public double Left
        {
            get { return GetValue<double>(nameof(Left)); }
            set { SetValue(nameof(Left), value); }
        }

        public double Width
        {
            get { return GetValue<double>(nameof(Width)); }
            set { SetValue(nameof(Width), value); }
        }
        public double Height
        {
            get { return GetValue<double>(nameof(Height)); }
            set { SetValue(nameof(Height), value); }
        }
        
        public bool Selected
        {
            get { return GetValue<bool>(nameof(Selected)); }
            set { SetValue(nameof(Selected), value); }
        }

        public Rect Bounds => new Rect(Left, Top, Width, Height);
       
    }
}

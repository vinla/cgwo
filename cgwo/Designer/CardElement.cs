using System;
using System.Windows;
using System.Windows.Input;
using GorgleDevs.Wpf.Mvvm;

namespace Cogs.Designer
{
    public abstract class CardElement : ViewModel
    {
		private Guid _id = Guid.NewGuid();

		public Guid Id => _id;

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

		public Rect Bounds => new Rect(Left, Top, Width, Height);

		public int ZIndex
        {
            get { return GetValue<int>(nameof(ZIndex)); }
            set { SetValue(nameof(ZIndex), value); }
        }

		public void SetBounds(Rect bounds)
		{
			Left = bounds.Left;
			Top = bounds.Top;
			Width = bounds.Width;
			Height = bounds.Height;
		}

		public bool Selected
		{
			get { return GetValue<bool>(nameof(Selected)); }
			set { SetValue(nameof(Selected), value); }
		}

		public object Clone()
		{
			return Clone(true);
		}

		public object Clone(bool cloneId)
		{
			var clone = MemberwiseClone() as CardElement;
			if (cloneId == false)
				clone._id = Guid.NewGuid();

			return clone;
		}

		public IZIndexManager ZIndexManager { get; set; }

        public ICommand SendToBack => new DelegateCommand(() => ZIndexManager?.SendToBack(this));
        public ICommand SendBackwards => new DelegateCommand(() => ZIndexManager?.SendBackwards(this));
        public ICommand BringToFront => new DelegateCommand(() => ZIndexManager?.BringToFront(this));
        public ICommand BringForwards => new DelegateCommand(() => ZIndexManager?.BringForwards(this));
    }
}

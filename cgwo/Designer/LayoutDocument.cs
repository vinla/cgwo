using System.Collections.ObjectModel;
using System.Windows.Media;
using GorgleDevs.Wpf.Mvvm;

namespace Cogs.Designer
{
	public class LayoutDocument : ViewModel
	{
		private readonly ObservableCollection<CardElement> _elements;

		public LayoutDocument()
		{
			_elements = new ObservableCollection<CardElement>();
		}

		public Color BackgroundColor
		{
			get { return GetValue<Color>(nameof(BackgroundColor)); }
			set { SetValue(nameof(BackgroundColor), value); }
		}

		public byte[] BackgroundImage
		{
			get { return GetValue<byte[]>(nameof(BackgroundImage)); }
			set { SetValue(nameof(BackgroundImage), value); }
		}

		[CalculateFrom(nameof(BackgroundColor))]
		[CalculateFrom(nameof(BackgroundImage))]
		public Brush Background
		{
			get
			{
				if (BackgroundImage != null && BackgroundImage.Length > 0)
				{
					return new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(BackgroundImage));
				}

				return new SolidColorBrush(BackgroundColor);
			}
		}

		public ObservableCollection<CardElement> Elements => _elements;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GorgleDevs.Wpf
{
	/// <summary>
	/// Interaction logic for AdornedControl.xaml
	/// </summary>
	public partial class AdornedControl : Decorator
	{
		private AdornerLayer _adornerLayer;
		private ElementAdorner _visualAdorner;
		public AdornedControl()
		{
			InitializeComponent();
			Loaded += (s, e) =>
			{
				_adornerLayer = AdornerLayer.GetAdornerLayer(this);
				_visualAdorner = new ElementAdorner(this);
				_visualAdorner.Element = Adornment;
				_visualAdorner.Visibility = IsAdornerVisible ? Visibility.Visible : Visibility.Hidden;

				if (Adornment != null)
					Adornment.DataContext = DataContext;

				_adornerLayer.Add(_visualAdorner);
			};
		}

		public static readonly DependencyProperty AdornmentProperty = DependencyProperty.Register(nameof(Adornment), typeof(FrameworkElement), typeof(AdornedControl), new FrameworkPropertyMetadata(AdornmentChangedCallback));

		public FrameworkElement Adornment
		{
			get { return (FrameworkElement)GetValue(AdornmentProperty); }
			set { SetValue(AdornmentProperty, value); }
		}

		public static readonly DependencyProperty IsAdornerVisibleProperty = DependencyProperty.Register(nameof(IsAdornerVisible), typeof(bool), typeof(AdornedControl), new FrameworkPropertyMetadata(false, IsAdornerVisibleChangedCallback));

		public bool IsAdornerVisible
		{
			get { return (bool)GetValue(IsAdornerVisibleProperty); }
			set { SetValue(IsAdornerVisibleProperty, value); }
		}

		private static void AdornmentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if(d is AdornedControl adornedControl)
			{
				if(adornedControl._visualAdorner != null)
					adornedControl._visualAdorner.Element = e.NewValue as FrameworkElement;
			}
		}

		private static void IsAdornerVisibleChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is AdornedControl adornedControl)
			{
				if (adornedControl._visualAdorner != null)
					adornedControl._visualAdorner.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Hidden;
			}
		}
	}
}

using System.Windows;
using System.Windows.Documents;
using System.Windows.Interactivity;

namespace GorgleDevs.Wpf.Behaviours
{
	public class AttachAdornerBehaviour : Behavior<FrameworkElement>
	{
		public static readonly DependencyProperty AdornerFactoryProperty = DependencyProperty.Register(nameof(AdornerFactory), typeof(AdornerFactory), typeof(AttachAdornerBehaviour));

		private Adorner _adorner;

		public AdornerFactory AdornerFactory
		{
			get { return (AdornerFactory)GetValue(AdornerFactoryProperty); }
			set { SetValue(AdornerFactoryProperty, value); }
		}

		protected override void OnAttached()
		{
			AssociatedObject.Loaded += (s, e) =>
			{
				_adorner = AdornerFactory.Create(AssociatedObject);
				var adornerLayer = AdornerLayer.GetAdornerLayer(AssociatedObject);
				adornerLayer.Add(_adorner);
			};
			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			var adornerLayer = AdornerLayer.GetAdornerLayer(AssociatedObject);
			adornerLayer.Remove(_adorner);
			_adorner = null;
			base.OnDetaching();
		}
	}

	public abstract class AdornerFactory : DependencyObject
	{
		public abstract Adorner Create(UIElement adornedElement);
	}
}

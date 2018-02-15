using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace GorgleDevs.Wpf
{
	public class ElementAdorner : Adorner
	{
		private FrameworkElement _element;
		public ElementAdorner(UIElement element) : base(element)
		{
		}

		public FrameworkElement Element
		{
			get { return _element; }
			set
			{
				if (_element != null)
					RemoveVisualChild(_element);

				_element = value;

				if (_element != null)
					AddVisualChild(_element);
			}
		}

		protected override Visual GetVisualChild(int index)
		{
			if (index > 1)
				throw new ArgumentOutOfRangeException();

			return _element;
		}

		protected override int VisualChildrenCount => 1;

		protected override Size ArrangeOverride(Size finalSize)
		{
			_element.Arrange(new Rect(new Point(0, 0), finalSize));
			return new Size(_element.ActualWidth, _element.ActualHeight);
		}

		protected override Size MeasureOverride(Size constraint)
		{
			return AdornedElement.RenderSize;
		}
	}
}

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using GorgleDevs.Wpf.Behaviours;

namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
	public class GridAdorner : Adorner
	{
		public GridAdorner(UIElement adornedElement) : base(adornedElement)
		{
			IsHitTestVisible = false;
		}		

		public Size GridSize
		{
			get; set;
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			int gridRows = (int)(AdornedElement.RenderSize.Height / GridSize.Height);
			int gridCols = (int)(AdornedElement.RenderSize.Width / GridSize.Width);

			var gridPen = new Pen(Brushes.GreenYellow, 1f);

			for (int i = 0; i < gridRows; i++)
				drawingContext.DrawLine(gridPen, new Point(0, i * GridSize.Height), new Point(AdornedElement.RenderSize.Width, i * GridSize.Height));
			for (int i = 0; i < gridCols; i++)
				drawingContext.DrawLine(gridPen, new Point(i * GridSize.Width, 0), new Point(i * GridSize.Width, AdornedElement.RenderSize.Height));

			base.OnRender(drawingContext);
		}
	}

	public class GridAdornerFactory : AdornerFactory
	{
		private GridAdorner _gridAdorner;
		public override Adorner Create(UIElement adornedElement)
		{
			_gridAdorner = new GridAdorner(adornedElement);
			_gridAdorner.GridSize = GridSize;
			return _gridAdorner;
		}
		
		public static readonly DependencyProperty GridSizeProperty = DependencyProperty.Register(nameof(GridSize), typeof(Size), typeof(GridAdornerFactory), new PropertyMetadata(GridSizePropertyChangedCallback));

		public Size GridSize
		{
			get { return (Size)GetValue(GridSizeProperty); }
			set { SetValue(GridSizeProperty, value); }
		}

		private static void GridSizePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is GridAdornerFactory factory && factory._gridAdorner != null)
			{
				factory._gridAdorner.GridSize = (Size)e.NewValue;
			}
		}
	}
}

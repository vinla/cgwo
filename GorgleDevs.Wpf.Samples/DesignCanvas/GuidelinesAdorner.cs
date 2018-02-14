using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using GorgleDevs.Wpf.Behaviours;

namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
	public class GuidelinesAdorner : Adorner
	{
		private readonly Guidelines _guidelines;
		public GuidelinesAdorner(UIElement adornedElement, Guidelines guidelines) : base(adornedElement)
		{
			IsHitTestVisible = false;
			_guidelines = guidelines;
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			var linePen = new Pen(Brushes.OrangeRed, 0.5f);
			linePen.DashStyle = DashStyles.Dash;

			var activeGuidelines = _guidelines.Where(gl => gl.IsActive);

			foreach(var line in activeGuidelines)
			{
				if (line.Orientation == GuideLineOrientation.Horizontal)
					drawingContext.DrawLine(linePen, new Point(0, line.Position), new Point(AdornedElement.RenderSize.Width, line.Position));
				else if(line.Orientation == GuideLineOrientation.Vertical)
					drawingContext.DrawLine(linePen, new Point(line.Position, 0), new Point(line.Position, AdornedElement.RenderSize.Height));
			}

			base.OnRender(drawingContext);
		}
	}

	public class GuidelinesAdornerFactory : AdornerFactory
	{
		public override Adorner Create(UIElement adornedElement)
		{
			return new GuidelinesAdorner(adornedElement, Guidelines);
		}

		public static readonly DependencyProperty GuidelinesProperty = DependencyProperty.Register(nameof(Guidelines), typeof(Guidelines), typeof(GuidelinesAdornerFactory));
		
		public Guidelines Guidelines
		{
			get { return (Guidelines)GetValue(GuidelinesProperty); }
			set { SetValue(GuidelinesProperty, value); }
		}
	}
}

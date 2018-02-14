using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

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
			var linePen = new Pen(Brushes.OrangeRed, 1f);
			linePen.DashStyle = DashStyles.Dash;

			var activeGuidelines = _guidelines.Where(gl => gl.IsActive);

			foreach (var line in activeGuidelines)
			{
				if (line.Orientation == GuideLineOrientation.Horizontal)
					drawingContext.DrawLine(linePen, new Point(0, line.Position), new Point(AdornedElement.RenderSize.Width, line.Position));
				else if (line.Orientation == GuideLineOrientation.Vertical)
					drawingContext.DrawLine(linePen, new Point(line.Position, 0), new Point(line.Position, AdornedElement.RenderSize.Height));
			}

			base.OnRender(drawingContext);
		}
	}
}

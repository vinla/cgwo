using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Cogs.Designer
{
    public class HighlightAdorner : Adorner
    {
        public HighlightAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var outlinePen = new Pen(Brushes.SteelBlue, 2);
            outlinePen.DashStyle = new DashStyle(new[] { 1d, 1d }, 0);
            var rect = new Rect(1, 1, AdornedElement.RenderSize.Width, RenderSize.Height);
            drawingContext.DrawRectangle(null, outlinePen, rect);

            base.OnRender(drawingContext);
        }
    }
}

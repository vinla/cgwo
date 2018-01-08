using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace GorgleDevs.Wpf
{
	public class DragFeedbackAdorner : Adorner
	{
		private DragDropOverlay _overlay;
		public DragFeedbackAdorner(UIElement adornedElement) : base(adornedElement)
		{
			_overlay = new DragDropOverlay();			
		}

		public void SetState(bool canDrop)
		{
			//_overlay.CanDrop = canDrop;
		}

		public void SetData(object data)
		{
			_overlay.DataContext = data;
			_overlay.Measure(AdornedElement.RenderSize);
		}
		protected override void OnRender(DrawingContext drawingContext)
		{
			var visualBrush = new VisualBrush(_overlay);

			var point = AdornedElement.PointFromScreen(DragDropManager.GetMousePosition());
			drawingContext.DrawRectangle(visualBrush, null, new Rect(point, _overlay.RenderSize));
			base.OnRender(drawingContext);
		}
	}
}

using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
	public class DragActionZ
	{
		private readonly List<LayoutElement> _draggedElements;
		private readonly Guidelines _guidelines;
		private Point _trackingPoint;		
		
		public DragActionZ(IEnumerable<LayoutElement> elements, Guidelines guidelines)
		{
			_draggedElements = elements.ToList();
			_guidelines = guidelines;			

			foreach(var el in _draggedElements)
			{
				var point = new Point(el.Left, el.Top);
				point = _guidelines.SnapPointToGrid(point);
				el.Left = point.X;
				el.Top = point.Y;
			}

			_trackingPoint = ElementsRect.TopRight;
		}

		public void Update(Vector delta)
		{
			_trackingPoint += delta;
			var snap = _guidelines.SnapPointToGrid(_trackingPoint);
			var snapDelta = snap - ElementsRect.TopRight;

			foreach (var el in _draggedElements)
			{
				el.Top += snapDelta.Y;
				el.Left += snapDelta.X;
			}
		}
		private Rect ElementsRect => Bounds.FromRectangles(_draggedElements.Select(el => el.Bounds));
	}	

	public static class Bounds
	{
		public static Rect FromRectangles(IEnumerable<Rect> rectangles)
		{
			var minX = rectangles.Min(el => el.Left);
			var minY = rectangles.Min(el => el.Top);
			var maxX = rectangles.Max(el => el.Right);
			var maxY = rectangles.Max(el => el.Bottom);

			return new Rect(new Point(minX, minY), new Point(maxX, maxY));
		}
	}
}

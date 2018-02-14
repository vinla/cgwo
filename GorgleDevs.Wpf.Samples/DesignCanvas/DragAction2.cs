using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
	public class DragAction2
	{
		private readonly Guidelines _guideLines;
		private readonly List<LayoutElement> _draggedElements;
		private Point _referencePoint;
		private Dictionary<Guid, Point> _startingPoints;
		private Rect _trackingRect; 

		public DragAction2(IEnumerable<LayoutElement> draggedElements, Guidelines guideLines)
		{
			_guideLines = guideLines;
			_draggedElements = draggedElements.ToList();
			_trackingRect = Bounds.FromRectangles(_draggedElements.Select(el => el.Bounds));
			_startingPoints = _draggedElements.ToDictionary(x => x.Id, x => x.Bounds.TopLeft);
			_referencePoint = _trackingRect.TopLeft;
		}

		public void Update(Vector delta)
		{
			_trackingRect.Offset(delta);
			var tempRect = _guideLines.SnapRectangle(_trackingRect);
			var overallDelta = tempRect.TopLeft - _referencePoint;			

			foreach (var el in _draggedElements)
			{
				el.Top = _startingPoints[el.Id].Y + overallDelta.Y;
				el.Left = _startingPoints[el.Id].X + overallDelta.X;
			}
		}
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

		public static Point CenterOf(Rect rect)
		{
			return new Point(rect.Left + rect.Width / 2f, rect.Top + rect.Height / 2f);
		}
	}
}

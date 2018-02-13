using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
	public class DragAction2
	{
		private readonly List<LayoutElement> _draggedElements;
		private Point _referencePoint;
		private Dictionary<Guid, Point> _startingPoints;
		private Rect _trackingRect; 

		public DragAction2(IEnumerable<LayoutElement> draggedElements)
		{
			_draggedElements = draggedElements.ToList();
			_trackingRect = Bounds.FromRectangles(_draggedElements.Select(el => el.Bounds));
			_startingPoints = _draggedElements.ToDictionary(x => x.Id, x => x.Bounds.TopLeft);
			_referencePoint = _trackingRect.TopLeft;
		}

		public void Update(Vector delta)
		{
			_trackingRect.Offset(delta);
			var overallDelta = _trackingRect.TopLeft - _referencePoint;			

			foreach (var el in _draggedElements)
			{
				el.Top = _startingPoints[el.Id].Y + overallDelta.Y;
				el.Left = _startingPoints[el.Id].X + overallDelta.X;
			}
		}
	}
}

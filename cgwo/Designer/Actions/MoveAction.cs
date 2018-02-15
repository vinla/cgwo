using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GorgleDevs.Wpf;

namespace Cogs.Designer.Actions
{
	public class MoveAction : DesignerAction
    {
		private readonly Guidelines _guidelines;
		private readonly List<CardElement> _elements;
		private Point _referencePoint;
		private Dictionary<Guid, Point> _startingPoints;
		private Dictionary<Guid, Point> _updatedPoints;
		private Rect _trackingRect;

		public MoveAction(IEnumerable<CardElement> elements, Guidelines guidelines)
		{
			_guidelines = guidelines;
			_elements = elements.ToList();
			_trackingRect = Bounds.FromRectangles(_elements.Select(el => el.Bounds));
			_startingPoints = _elements.ToDictionary(x => x.Id, x => x.Bounds.TopLeft);
			_referencePoint = _trackingRect.TopLeft;
		}

		public void Update(Vector moveVector)
		{
			_trackingRect.Offset(moveVector);
			var tempRect = _guidelines.SnapRectangle(_trackingRect);
			var overallDelta = tempRect.TopLeft - _referencePoint;

			foreach (var el in _elements)
			{
				el.Top = _startingPoints[el.Id].Y + overallDelta.Y;
				el.Left = _startingPoints[el.Id].X + overallDelta.X;
			}

			_updatedPoints = _elements.ToDictionary(x => x.Id, x => x.Bounds.TopLeft);
		}

		public override void Undo()
		{
			_elements.ForEach(el =>
			{
				var startingPoint = _startingPoints[el.Id];
				el.Left = startingPoint.X;
				el.Top = startingPoint.Y;
			});
		}

		public override void Redo()
		{
			_elements.ForEach(el =>
			{
				var endPoint = _updatedPoints[el.Id];
				el.Left = endPoint.X;
				el.Top = endPoint.Y;
			});
		}

		protected override void AfterCompleted()
		{
			_guidelines.HideAll();
		}
	}
}

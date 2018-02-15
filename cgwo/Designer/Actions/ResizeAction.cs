using System;
using System.Windows;
using GorgleDevs.Wpf;

namespace Cogs.Designer.Actions
{
	public class ResizeAction : DesignerAction
    {
		private readonly Guidelines _guidelines;
		private readonly Rect _original;		
        private readonly CardElement _element;
		private readonly string _direction;
		private Rect _final;
		private Point _trackingPoint;
		private Point _referencePoint;

        public ResizeAction(CardElement element, string direction, Guidelines guidelines)
        {
            _element = element;
			_direction = direction;
			_original = element.Bounds;
			_final = element.Bounds;
			_guidelines = guidelines;
			_trackingPoint = GetTrackingPoint(element, direction);
			_referencePoint = _trackingPoint;
        }

        public void Update(Vector delta)
        {
			var tempPoint = _trackingPoint;

			if (_direction.Contains("Left") || _direction.Contains("Right"))
			{
				_trackingPoint.Offset(delta.X, 0);
				tempPoint.X = _guidelines.SnapVerticalEdge(_trackingPoint.X);
			}

			if(_direction.Contains("Bottom") || _direction.Contains("Top"))
			{
				_trackingPoint.Offset(0, delta.Y);
				tempPoint.Y = _guidelines.SnapHorizontalEdge(_trackingPoint.Y);
			}

			_guidelines.Update();

			var overallDelta = tempPoint - _referencePoint;

			if (_direction.Contains("Right"))
			{				
				_element.Width = Math.Max(_original.Width + overallDelta.X, 1);
			}

			if (_direction.Contains("Bottom"))
			{
				_element.Height = Math.Max(_original.Height + overallDelta.Y, 1);
			}

			if (_direction.Contains("Left"))
			{
				double newWidth = Math.Max(_original.Width - overallDelta.X, 1);

				_element.Left = _element.Left - (newWidth - _element.Width);
				_element.Width = newWidth;
			}

			if (_direction.Contains("Top"))
			{

				double newHeight = Math.Max(_original.Height - overallDelta.Y, 1);

				_element.Top = _element.Top - (newHeight - _element.Height);
				_element.Height = newHeight;
			}

			_final = _element.Bounds;
        }

		public override void Undo()
		{
			_element.SetBounds(_original);
		}

		public override void Redo()
		{
			_element.SetBounds(_final);
		}

		protected override void AfterCompleted()
		{
			_guidelines.HideAll();
		}

		private static Point GetTrackingPoint(CardElement element, string direction)
		{
			var point = Bounds.CenterOf(element.Bounds);
			if (direction.Contains("Top"))
				point.Y = element.Top;

			if (direction.Contains("Bottom"))
				point.Y = element.Bounds.Bottom;

			if (direction.Contains("Left"))
				point.X = element.Left;

			if (direction.Contains("Right"))
				point.X = element.Bounds.Right;

			return point;
		}
	}	
}

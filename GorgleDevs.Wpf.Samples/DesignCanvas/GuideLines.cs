using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
	public class Guidelines : IEnumerable<Guideline>
	{
		private double _snapDistance;
		private List<Guideline> _guidelines;
		private Func<double, bool> _inSnapDistance;

		public Guidelines()
		{
			_guidelines = new List<Guideline>();			
			_snapDistance = 3;
			_inSnapDistance = (delta) => Math.Abs(delta) < _snapDistance;
		}

		public void GenerateGuidelines(IEnumerable<Rect> rectangles)
		{
			_guidelines.Clear();
			foreach(var rect in rectangles)
			{
				var centerPoint = Bounds.CenterOf(rect);
				_guidelines.Add(new Guideline { Position = rect.Top, Orientation = GuideLineOrientation.Horizontal, Type = GuidelineType.Edge });
				_guidelines.Add(new Guideline { Position = rect.Bottom, Orientation = GuideLineOrientation.Horizontal, Type = GuidelineType.Edge });
				_guidelines.Add(new Guideline { Position = centerPoint.Y, Orientation = GuideLineOrientation.Horizontal, Type = GuidelineType.Center });
				_guidelines.Add(new Guideline { Position = rect.Left, Orientation = GuideLineOrientation.Vertical, Type = GuidelineType.Edge });
				_guidelines.Add(new Guideline { Position = rect.Right, Orientation = GuideLineOrientation.Vertical, Type = GuidelineType.Edge });
				_guidelines.Add(new Guideline { Position = centerPoint.X, Orientation = GuideLineOrientation.Vertical, Type = GuidelineType.Center });
			}
			OnLinesUpdated();
		}

		public IEnumerator<Guideline> GetEnumerator()
		{
			return _guidelines.GetEnumerator();
		}

		public Rect SnapRectangle(Rect rect)
		{
			var vertResults = GetVerticalSnap(rect);
			var overallResult = GetHorizontalSnap(vertResults);
			OnLinesUpdated();
			return overallResult;
		}

		public void HideAll()
		{
			_guidelines.ForEach(gl => gl.IsActive = false);
			OnLinesUpdated();
		}

		private Rect GetVerticalSnap(Rect rect)
		{
			var results = new List<SnapResult>();

			foreach (var guideline in _guidelines.Where(gl => gl.Orientation == GuideLineOrientation.Horizontal && gl.Type == GuidelineType.Edge))
			{
				guideline.IsActive = false;
				var delta = rect.Top - guideline.Position;
				if (_inSnapDistance(delta))
				{
					results.Add(new SnapResult
					{
						GuideLine = guideline,
						Delta = delta,
						SnappedRect = new Rect(rect.Left, guideline.Position, rect.Width, rect.Height)
					});
				}

				delta = rect.Bottom - guideline.Position;
				if(_inSnapDistance(delta))
				{
					results.Add(new SnapResult
					{
						GuideLine = guideline,
						Delta = delta,
						SnappedRect = new Rect(rect.Left, guideline.Position - rect.Height, rect.Width, rect.Height)
					});
				}
			}

			foreach (var guideline in _guidelines.Where(gl => gl.Orientation == GuideLineOrientation.Horizontal && gl.Type == GuidelineType.Center))
			{
				guideline.IsActive = false;
				var centerLine = Bounds.CenterOf(rect).Y;

				var delta = centerLine - guideline.Position;
				if (_inSnapDistance(delta))
				{
					results.Add(new SnapResult
					{
						GuideLine = guideline,
						Delta = delta,
						SnappedRect = new Rect(rect.Left, guideline.Position - rect.Height / 2f, rect.Width, rect.Height)
					});
				}
			}

			if (results.Any())
			{
				var closest = results.OrderBy(r => r.Delta).First();

				foreach (var result in results.Where(r => r.Delta == closest.Delta))
					result.GuideLine.IsActive = true;
				
				return closest.SnappedRect;
			}

			return rect;
		}

		private Rect GetHorizontalSnap(Rect rect)
		{
			var results = new List<SnapResult>();

			foreach (var guideline in _guidelines.Where(gl => gl.Orientation == GuideLineOrientation.Vertical && gl.Type == GuidelineType.Edge))
			{
				guideline.IsActive = false;
				var delta = rect.Left - guideline.Position;
				if (_inSnapDistance(delta))
				{
					results.Add(new SnapResult
					{
						GuideLine = guideline,
						Delta = delta,
						SnappedRect = new Rect(guideline.Position, rect.Top, rect.Width, rect.Height)
					});
				}

				delta = rect.Right - guideline.Position;
				if (_inSnapDistance(delta))
				{
					results.Add(new SnapResult
					{
						GuideLine = guideline,
						Delta = delta,
						SnappedRect = new Rect(guideline.Position - rect.Width, rect.Top, rect.Width, rect.Height)
					});
				}
			}

			foreach (var guideline in _guidelines.Where(gl => gl.Orientation == GuideLineOrientation.Vertical && gl.Type == GuidelineType.Center))
			{
				guideline.IsActive = false;
				var centerLine = Bounds.CenterOf(rect).X;

				var delta = centerLine - guideline.Position;
				if (_inSnapDistance(delta))
				{
					results.Add(new SnapResult
					{
						GuideLine = guideline,
						Delta = delta,
						SnappedRect = new Rect(guideline.Position - rect.Width / 2f, rect.Top, rect.Width, rect.Height)
					});
				}
			}

			if (results.Any())
			{
				var closest = results.OrderBy(r => r.Delta).First();
				foreach (var result in results.Where(r => r.Delta == closest.Delta))
					result.GuideLine.IsActive = true;
				return closest.SnappedRect;
			}

			return rect;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _guidelines.GetEnumerator();
		}

		private class SnapResult
		{
			public Guideline GuideLine { get; set; }
			public double Delta { get; set; }
			public Rect SnappedRect { get; set; }
		}

		public event EventHandler LinesUpdated;

		protected virtual void OnLinesUpdated()
		{
			LinesUpdated?.Invoke(this, EventArgs.Empty);
		}
	}	

	public class Guideline
	{
		public double Position { get; set; }
		public GuideLineOrientation Orientation { get; set; }		
		public GuidelineType Type { get; set; }
		public bool IsActive { get; set; }
	}

	public enum GuideLineOrientation
	{
		Vertical,
		Horizontal
	}

	public enum GuidelineType
	{
		Edge,
		Center
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
	public class GuideLines
	{
		private double _snapDistance;
		private List<GuideLine> _guideLines;

		public GuideLines()
		{
			_guideLines = new List<GuideLine>();
			_guideLines.Add(new GuideLine { Position = 50, Orientation = GuideLineOrientation.Horizontal });
			_snapDistance = 2;
		}

		public Rect SnapRectangle(Rect rect)
		{
			var results = new List<SnapResult>();

			foreach (var guideline in _guideLines.Where(gl => gl.Orientation == GuideLineOrientation.Horizontal))
			{
				var delta = rect.Top - guideline.Position;
				if(Math.Abs(delta) <= _snapDistance)
				{
					results.Add(new SnapResult
					{
						GuideLine = guideline,
						Delta = delta,
						SnappedRect = new Rect(rect.Left, guideline.Position, rect.Width, rect.Height)
					});
				}
			}

			if (results.Any())
				return results.OrderBy(r => r.Delta).First().SnappedRect;

			return rect;
		}

		private class SnapResult
		{
			public GuideLine GuideLine { get; set; }
			public double Delta { get; set; }
			public Rect SnappedRect { get; set; }
		}
	}	

	public class GuideLine
	{
		public double Position { get; set; }
		public GuideLineOrientation Orientation { get; set; }		
	}

	public enum GuideLineOrientation
	{
		Vertical,
		Horizontal
	}
}

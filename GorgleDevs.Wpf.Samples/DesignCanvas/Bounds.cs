using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
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

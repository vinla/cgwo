using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GorgleDevs.Wpf
{
	public abstract class ResizeEventArgs : EventArgs
	{
		private ResizeHandlePosition _position;

		protected ResizeEventArgs(ResizeHandlePosition position)
		{
			_position = position;
		}

		public ResizeHandlePosition Position => _position;
	}

	public class ResizeStartedEventArgs : ResizeEventArgs
	{
		public ResizeStartedEventArgs(ResizeHandlePosition position) : base(position)
		{
		}
	}

	public class ResizeDeltaEventArgs : ResizeEventArgs
	{
		private readonly Vector _delta;
		private ResizeHandlePosition resizeHandle;
		private Vector vector;

		public ResizeDeltaEventArgs(ResizeHandlePosition position, Vector delta) : base(position)
		{
			_delta = delta;
		}
		public Vector Delta => _delta;
	}

	public class ResizeCompletedEventArgs : ResizeEventArgs
	{
		public ResizeCompletedEventArgs(ResizeHandlePosition position) : base(position)
		{
		}
	}

	public enum ResizeHandlePosition
	{
		TopLeft,
		Top,
		TopRight,
		Right,
		BottomRight,
		Bottom,
		BottomLeft,
		Left
	}
}

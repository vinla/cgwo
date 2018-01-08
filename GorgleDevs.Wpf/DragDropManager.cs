using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Documents;

namespace GorgleDevs.Wpf
{
	public static class DragDropManager
	{
		private static UIElement _trackingElement;
		private static AdornerLayer _layer;
		private static DragFeedbackAdorner _adorner;
		private static object _dragData;

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetCursorPos(ref Win32Point pt);

		[StructLayout(LayoutKind.Sequential)]
		internal struct Win32Point
		{
			public Int32 X;
			public Int32 Y;
		};

		public static object DragData => _dragData;		

		public static void SetTrackingElement(UIElement trackingElement)
		{
			_trackingElement = trackingElement;			
		}

		private static void InitialiseLayer()
		{
			if (_layer == null && _trackingElement != null)
			{

				_layer = AdornerLayer.GetAdornerLayer(_trackingElement);
				_adorner = new DragFeedbackAdorner(_trackingElement);
				_adorner.IsHitTestVisible = false;
				_layer.IsHitTestVisible = false;
				_layer.Add(_adorner);
			}
		}

		public static Point GetMousePosition()
		{
			Win32Point w32Mouse = new Win32Point();
			GetCursorPos(ref w32Mouse);
			return new Point(w32Mouse.X, w32Mouse.Y);
		}

		public static void StartDrag(object dragData)
		{
			InitialiseLayer();
			_adorner?.SetData(dragData);
			_dragData = dragData;
		}

		public static void StopDrag()
		{
			_dragData = null;
			_adorner?.SetData(null);
		}

		public static void UpdateDrag(DragDropEffects effects)
		{
			_adorner?.SetState(effects != DragDropEffects.None);
			_layer?.Update();
		}
	}

    public interface IDragSource
    {
        bool CanDrag { get; }
        object StartDrag();
        void StopDrag();
    }
}

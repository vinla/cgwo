using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GorgleDevs.Wpf
{
	/// <summary>
	/// Interaction logic for ResizeOverlay.xaml
	/// </summary>
	public partial class ResizeOverlay : UserControl
	{
		public ResizeOverlay()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty ThumbStyleProperty = DependencyProperty.Register(nameof(ThumbStyle), typeof(Style), typeof(ResizeOverlay));

		public Style ThumbStyle
		{
			get { return (Style)GetValue(ThumbStyleProperty); }
			set { SetValue(ThumbStyleProperty, value); }
		}

		public event EventHandler<ResizeStartedEventArgs> ResizeStarted;

		public event EventHandler<ResizeDeltaEventArgs> ResizeDelta;

		public event EventHandler<ResizeCompletedEventArgs> ResizeCompleted;

		private void ThumbDragCompleted(object sender, DragCompletedEventArgs e)
		{
			if (sender is Thumb thumb)
			{
				var resizeHandle = (ResizeHandlePosition)Enum.Parse(typeof(ResizeHandlePosition), (string)thumb.Tag);
				ResizeCompleted?.Invoke(this, new ResizeCompletedEventArgs(resizeHandle));
			}
		}

		private void ThumbDragStarted(object sender, DragStartedEventArgs e)
		{
			if (sender is Thumb thumb)
			{
				var resizeHandle = (ResizeHandlePosition)Enum.Parse(typeof(ResizeHandlePosition), (string)thumb.Tag);
				ResizeStarted?.Invoke(this, new ResizeStartedEventArgs(resizeHandle));
			}
		}

		private void ThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			if (sender is Thumb thumb)
			{
				var resizeHandle = (ResizeHandlePosition)Enum.Parse(typeof(ResizeHandlePosition), (string)thumb.Tag);
				ResizeDelta?.Invoke(this, new ResizeDeltaEventArgs(resizeHandle, new Vector(e.HorizontalChange, e.VerticalChange)));
			}
		}
	}
}

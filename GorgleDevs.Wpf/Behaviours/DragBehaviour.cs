using System.Windows;
using System.Windows.Interactivity;

namespace GorgleDevs.Wpf.Behaviours
{
	public class DragBehaviour : Behavior<FrameworkElement>
	{
        public static readonly DependencyProperty DragDataProperty = DependencyProperty.Register(nameof(DragData), typeof(object), typeof(DragBehaviour));

        public object DragData
        {
            get { return GetValue(DragDataProperty); }
            set { SetValue(DragDataProperty, value); }
        }

        public static readonly DependencyProperty CanDragProperty = DependencyProperty.Register(nameof(CanDrag), typeof(bool), typeof(DragBehaviour));

        public bool CanDrag
        {
            get { return (bool)GetValue(CanDragProperty); }
            set { SetValue(CanDragProperty, value); }
        }

		private bool _mouseDown;
		protected override void OnAttached()
		{
			var mainWindow = Application.Current.MainWindow;
			base.OnAttached();

			AssociatedObject.MouseLeftButtonDown += (s, e) =>
			{
				_mouseDown = true;
			};

			AssociatedObject.MouseLeftButtonUp += (s, e) =>
			{
				_mouseDown = false;				
			};

			AssociatedObject.MouseMove += (s, e) =>
			{
				if (_mouseDown)
				{				
					
					if (CanDrag)
					{						
						DragDropManager.StartDrag(DragData);
						DragDrop.DoDragDrop(AssociatedObject, DragData, DragDropEffects.All);
						DragDropManager.StopDrag();						
						_mouseDown = false;
					}
				}
			};

			AssociatedObject.GiveFeedback += (s, e) =>
			{
				DragDropManager.UpdateDrag(e.Effects);
				//e.UseDefaultCursors = false;
				e.Handled = true;
			};
		}
	}	
}

using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GorgleDevs.Wpf.Behaviours
{
	public class DropBehavior : Behavior<FrameworkElement>
	{
        public static readonly DependencyProperty DropCommandProperty = DependencyProperty.Register(nameof(DropCommand), typeof(ICommand), typeof(DropBehavior));

        public ICommand DropCommand
        {
            get { return (ICommand)GetValue(DropCommandProperty); }
            set { SetValue(DropCommandProperty, value); }
        }

		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.AllowDrop = true;

			AssociatedObject.DragOver += (s, e) =>
			{
				if ((DropCommand?.CanExecute(DragDropManager.DragData)).GetValueOrDefault())
					e.Effects = DragDropEffects.Move;
				else
					e.Effects = DragDropEffects.None;

				e.Handled = true;					
			};			

			AssociatedObject.Drop += (s, e) =>
			{
                if ((DropCommand?.CanExecute(DragDropManager.DragData)).GetValueOrDefault())
                {
                    DropCommand?.Execute(DragDropManager.DragData);
                }                    

				e.Handled = true;
			};
		}
	}			
}

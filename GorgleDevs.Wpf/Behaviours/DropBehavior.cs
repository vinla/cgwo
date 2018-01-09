using System.Windows;
using System.Windows.Data;
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

        public static readonly DependencyProperty ConverterProperty = DependencyProperty.Register(nameof(Converter), typeof(IValueConverter), typeof(DropBehavior));

        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(ConverterProperty); }
            set { SetValue(ConverterProperty, value); }
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
                    DropCommand?.Execute(GetConvertedData(DragDropManager.DragData));
                }                    

				e.Handled = true;
			};
		}

        private object GetConvertedData(object originalData)
        {
            var data = originalData;
            if (Converter != null)
            {
                var offset = AssociatedObject.PointFromScreen(DragDropManager.GetMousePosition());
                data = Converter.Convert(data, typeof(DropBehavior), offset, System.Globalization.CultureInfo.CurrentCulture);
            }
            return data;
        }
	}			
}

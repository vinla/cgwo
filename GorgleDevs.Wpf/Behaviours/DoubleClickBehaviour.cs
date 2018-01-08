using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GorgleDevs.Wpf.Behaviours
{
    public class DoubleClickBehaviour : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(DoubleClickBehaviour));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(DoubleClickBehaviour));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += Click;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= Click;
            base.OnDetaching();
        }

        private void Click(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                Command?.Execute(CommandParameter);
            }
        }
    }
}

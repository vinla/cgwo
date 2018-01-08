using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GorgleDevs.Wpf.Behaviours
{
	public class KeyPreviewBehaviour : Behavior<FrameworkElement>
	{
		public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(nameof(Key), typeof(Key), typeof(KeyPreviewBehaviour));

		public Key Key
		{
			get { return (Key)GetValue(KeyProperty); }
			set { SetValue(KeyProperty, value); }
		}

		public static readonly DependencyProperty ModifiersProperty = DependencyProperty.Register(nameof(Modifiers), typeof(ModifierKeys), typeof(KeyPreviewBehaviour));

		public ModifierKeys Modifiers
		{
			get { return (ModifierKeys)GetValue(ModifiersProperty); }
			set { SetValue(ModifiersProperty, value); }
		}

		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(KeyPreviewBehaviour));

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(KeyPreviewBehaviour));

		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		protected override void OnAttached()
		{
			AssociatedObject.PreviewKeyDown += Check;
			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			AssociatedObject.PreviewKeyDown -= Check;
			base.OnDetaching();
		}

		private void Check(object sender, KeyEventArgs e)
		{
			if (e.Key == Key && (Keyboard.Modifiers & Modifiers) == Modifiers )
			{
				Command?.Execute(CommandParameter);
				e.Handled = true;
			}
		}
	}
}

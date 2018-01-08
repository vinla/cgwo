using System;
using System.Windows.Input;

namespace GorgleDevs.Wpf.Mvvm
{
	public class DelegateCommand : ICommand
	{
		private readonly Action<object> _action;

		public DelegateCommand(Action<object> action)
		{
			_action = action ?? throw new ArgumentNullException(nameof(action));
		}

		public DelegateCommand(Action action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));
			_action = (o) => action();
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_action(parameter);
		}
	}
}

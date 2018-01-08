using System.Collections.Generic;

namespace GorgleDevs.Wpf.Mvvm
{
	public class ViewModelStack : ViewModel
	{
		private readonly Stack<ViewModel> _stack;

		public ViewModelStack()
		{
			_stack = new Stack<ViewModel>();
		}

		public ViewModel Current => _stack.Peek();

		public void Push(ViewModel viewModel)
		{
			_stack.Push(viewModel);
			RaisePropertyChanged(nameof(Current)); 
		}

		public void Pop()
		{
			_stack.Pop();
			RaisePropertyChanged(nameof(Current));
		}
	}
}

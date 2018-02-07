using System.Collections.ObjectModel;
using GorgleDevs.Wpf.Mvvm;

namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
	public class LayoutDocument : ViewModel
	{
		private readonly ObservableCollection<LayoutElement> _elements;

		public LayoutDocument()
		{
			_elements = new ObservableCollection<LayoutElement>();			
		}

		public ObservableCollection<LayoutElement> Elements => _elements;
	}
}

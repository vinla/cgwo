using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GorgleDevs.Wpf.Mvvm;
using GorgleDevs.Wpf.Samples.DesignCanvas;

namespace GorgleDevs.Wpf.Samples
{
	public class MainViewModel : Mvvm.ViewModel
	{
		private readonly IEnumerable<LayoutElement> _elements;
		public MainViewModel()
		{			
			_elements = new[]
			{
				new DesignCanvas.LayoutElement{ Left = 105, Top = 80, Width = 100, Height = 25},
				new DesignCanvas.LayoutElement{ Left = 96, Top = 290, Width = 25, Height = 25},
				new DesignCanvas.LayoutElement{ Left = 196, Top = 190, Width = 50, Height = 125}
			};
			GridSize = new Size(10, 10);
		}
		public IEnumerable<LayoutElement> Elements => _elements;

		public Size GridSize
		{
			get { return GetValue<Size>(nameof(GridSize)); }
			set { SetValue(nameof(GridSize), value); }
		}

		public ICommand IncreaseGrid => new DelegateCommand(() =>
		{
			GridSize = new Size(GridSize.Width + 10, GridSize.Height + 10);
		});
	}
}

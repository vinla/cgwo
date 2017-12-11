using System;
using System.Collections.Generic;
using System.Linq;
using cgwo.Mvvm;

namespace cgwo.ViewModels
{
	public class MainViewModel : ViewModel
	{
		public static MainViewModel Current { get; private set; }

		private ViewModel _viewModel;
		private readonly Core.ProjectManager _projectManager;

		public MainViewModel(Core.ProjectManager projectManager)
		{
			_projectManager = projectManager ?? throw new ArgumentNullException(nameof(projectManager));
			_viewModel = new HomePageViewModel();			
			
			projectManager.ProjectLoaded += (s, e) =>
			{
				_viewModel = new ProjectViewModel();
				RaisePropertyChanged(nameof(CurrentViewModel));
			};
			
			Current = this;
		}

		public ViewModel CurrentViewModel => _viewModel;		

		public void Clear()
		{
			_viewModel = null;
			RaisePropertyChanged(nameof(CurrentViewModel));
		}
	}
}

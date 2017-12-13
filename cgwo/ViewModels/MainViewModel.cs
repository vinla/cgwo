using System;
using System.Collections.Generic;
using System.Linq;
using cgwo.Mvvm;
using Cogs.Common;

namespace cgwo.ViewModels
{
	public class MainViewModel : ViewModel
	{
		public static MainViewModel Current { get; private set; }
        private ViewModel _viewModel;		

		public MainViewModel(ICardGameDataStoreFactory dataStoreFactory)
		{			
			_viewModel = new HomePageViewModel(dataStoreFactory);
			Current = this;
		}

		public ViewModel CurrentViewModel => _viewModel;		

		public void Clear()
		{
			_viewModel = null;
			RaisePropertyChanged(nameof(CurrentViewModel));
		}

        public void ProjectLoaded(ICardGameDataStore dataStore)
        {
            _viewModel = new ProjectViewModel(dataStore);
			RaisePropertyChanged(nameof(CurrentViewModel));
        }
	}
}

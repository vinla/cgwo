using System;
using System.Collections.Generic;
using System.Linq;
using cgwo.Mvvm;
using Cogs.Common;

namespace cgwo.ViewModels
{
	public class MainViewModel : ViewModel
	{		
        private ViewModel _viewModel;		

		public MainViewModel(ICardGameDataStoreFactory dataStoreFactory)
		{			
			_viewModel = new HomePageViewModel(dataStoreFactory, ProjectLoaded);
		}

		public ViewModel CurrentViewModel => _viewModel;		
        
        public void ProjectLoaded(ICardGameDataStore dataStore)
        {
            _viewModel = new ProjectViewModel(dataStore);            
        }
	}
}

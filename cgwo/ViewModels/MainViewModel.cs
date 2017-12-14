using System;
using System.Collections.Generic;
using System.Linq;
using cgwo.Mvvm;
using Cogs.Common;

namespace cgwo.ViewModels
{
	public class MainViewModel : ViewModel
	{
        private readonly IDialogService _dialogService;
        private ViewModel _viewModel;		

		public MainViewModel(ICardGameDataStoreFactory dataStoreFactory, IDialogService dialogService)
		{
            _dialogService = dialogService ?? throw new ArgumentNullException();
			_viewModel = new HomePageViewModel(dataStoreFactory, _dialogService, ProjectLoaded);
		}

		public ViewModel CurrentViewModel => _viewModel;		
        
        public void ProjectLoaded(ICardGameDataStore dataStore)
        {
            _viewModel = new ProjectViewModel(dataStore, _dialogService);
			RaisePropertyChanged(nameof(CurrentViewModel));
        }
	}
}

using System;
using Cogs.Common;
using GorgleDevs.Wpf.Mvvm;

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
			Project.DataStore = dataStore;
            _viewModel = new ProjectViewModel(dataStore, _dialogService);
			RaisePropertyChanged(nameof(CurrentViewModel));
        }
	}
}

using System;
using System.Collections.Generic;
using System.Windows.Input;
using cgwo.Mvvm;
using Cogs.Common;

namespace cgwo.ViewModels
{
	public class HomePageViewModel : ViewModel
	{
		public string Title => "Welcome to Cogs 0.1";

        private readonly ICardGameDataStoreFactory _dataStoreFactory;
        private readonly Action<ICardGameDataStore> _onDataStoreLoad;
        private readonly IDialogService _dialogService;
		private ViewModel _childViewModel;

        public HomePageViewModel(ICardGameDataStoreFactory dataStoreFactory, IDialogService dialogService, Action<ICardGameDataStore> onDataStoreLoad)
        {
            _dialogService = dialogService;
            _dataStoreFactory = dataStoreFactory ?? throw new ArgumentNullException(nameof(dataStoreFactory));
            _onDataStoreLoad = onDataStoreLoad ?? throw new ArgumentNullException(nameof(onDataStoreLoad));
        }

		public ICommand NewProject => new DelegateCommand(() =>
		{
			var newProjectViewModel = new NewProjectViewModel(_dataStoreFactory, _dialogService, _onDataStoreLoad);
            newProjectViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(newProjectViewModel.Cancelled)
                    && newProjectViewModel.Cancelled
                    && _childViewModel == newProjectViewModel)
                {
                    _childViewModel = null;
                    RaisePropertyChanged(nameof(ChildViewModel));
                }
            };
            _childViewModel = newProjectViewModel;
			RaisePropertyChanged(nameof(ChildViewModel));
		});

		public ICommand Exit => new DelegateCommand(() =>
		{
			System.Windows.Application.Current.Shutdown();
		});

		public ICommand LoadProject => new DelegateCommand(() =>
		{
            var dlg = _dialogService.ChooseFile("c:\\utilities", "Card Project|*.dcproj");
            if(dlg.Result == DialogResult.Accept)
            {
                var parameters = new Dictionary<string, string>
                    {
                        { "FilePath", dlg.Path }
                    };

                try
                {
                    var dataStore = _dataStoreFactory.Open(parameters);
                    _onDataStoreLoad?.Invoke(dataStore);
                }
                catch
                {
                    _dialogService.Message("An error occurred trying to load the project file. Make sure you have chosen a valid project.");
                }
            }			
		});

		public ViewModel ChildViewModel => _childViewModel;
	}
}

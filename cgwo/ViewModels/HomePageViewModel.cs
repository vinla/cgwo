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
		private ViewModel _childViewModel;

        public HomePageViewModel(ICardGameDataStoreFactory dataStoreFactory, Action<ICardGameDataStore> onDataStoreLoad)
        {
            _dataStoreFactory = dataStoreFactory ?? throw new ArgumentNullException(nameof(dataStoreFactory));
            _onDataStoreLoad = onDataStoreLoad ?? throw new ArgumentNullException(nameof(onDataStoreLoad));
        }

		public ICommand NewProject => new DelegateCommand(() =>
		{
			var newProjectViewModel = new NewProjectViewModel(_dataStoreFactory, _onDataStoreLoad);
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
			using (var dlg = new System.Windows.Forms.OpenFileDialog())
			{
				dlg.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
				dlg.Filter = "Card Project|*.dcproj";
				if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
                    var parameters = new Dictionary<string, string>
                    {
                        { "FilePath", dlg.FileName }
                    };

                    var dataStore = _dataStoreFactory.Open(parameters);
                    _onDataStoreLoad?.Invoke(dataStore);
				}
			}
		});

		public ViewModel ChildViewModel => _childViewModel;
	}
}

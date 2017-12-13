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
		private ViewModel _childViewModel;

        public HomePageViewModel(ICardGameDataStoreFactory dataStoreFactory)
        {
            _dataStoreFactory = dataStoreFactory ?? throw new ArgumentNullException(nameof(dataStoreFactory));
        }

		public ICommand NewProject => new DelegateCommand(() =>
		{
			_childViewModel = new NewProjectViewModel(_dataStoreFactory);
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
                    MainViewModel.Current.ProjectLoaded(dataStore);
				}
			}
		});

		public ViewModel ChildViewModel => _childViewModel;
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Cogs.Common;
using GorgleDevs.Wpf.Mvvm;

namespace cgwo.ViewModels
{
	public class NewProjectViewModel: ViewModel
	{
        private readonly Action<ICardGameDataStore> _onDataStoreLoad;
        private readonly IDialogService _dialogService;
		private readonly ICardGameDataStoreFactory _dataStoreFactory;
		public NewProjectViewModel(ICardGameDataStoreFactory dataStoreFactory, IDialogService dialogService, Action<ICardGameDataStore> onDataStoreLoad)
		{
            _onDataStoreLoad = onDataStoreLoad;
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _dataStoreFactory = dataStoreFactory ?? throw new ArgumentNullException(nameof(dataStoreFactory));
			ErrorsChanged += (s, e) => RaisePropertyChanged(nameof(IsValid));			
		}

		public ICommand Cancel => new DelegateCommand(() =>
		{
            Cancelled = true;
		});

        public bool Cancelled
        {
            get { return GetValue<bool>(nameof(Cancelled)); }
            private set { SetValue(nameof(Cancelled), true); }
        }

		public ICommand Browse => new DelegateCommand(() =>
		{
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var dialog = _dialogService.ChooseFolder(myDocuments);

            if(dialog.Result == DialogResult.Accept)
            {
                SaveLocation = dialog.Path;
            }			
		});

        public ICommand Create => new DelegateCommand(() =>
        {
            if (Path.IsPathRooted(SaveLocation))
            {
                var path = Path.Combine(SaveLocation, $"{ProjectName}.dcproj");

                if (File.Exists(path))
                {
                    if (_dialogService.Prompt("Overwrite", "A project with this name already exists in this folder, do you want to overwrite it?") == DialogResult.Reject)
                        return;

                    try
                    {
                        File.Delete(path);
                    }
                    finally
                    {
                    }

                    if (File.Exists(path))
                    {
                        _dialogService.Message("Could not overwrite the existing file");
                        return;
                    }
                }

                var parameters = new Dictionary<string, string>
                {
                    {"FilePath", path }
                };

                var dataStore = _dataStoreFactory.Create(parameters);
                dataStore.SetProjectInfo(new ProjectInfo { Name = ProjectName });
                _onDataStoreLoad?.Invoke(dataStore);
            }
        });

		public string SaveLocation
		{
			get { return GetValue<string>(nameof(SaveLocation)); }
			set { SetValue(nameof(SaveLocation), value); }
		}

		public string ProjectName
		{
			get { return GetValue<string>(nameof(ProjectName)); }
			set { SetValue(nameof(ProjectName), value); }
		}

        [CalculateFrom(nameof(ProjectName))]
        [CalculateFrom(nameof(SaveLocation))]
		public bool IsValid =>
			HasErrors == false
			&& String.IsNullOrEmpty(ProjectName) == false
			&& String.IsNullOrEmpty(SaveLocation) == false;

		[ValidationFor(nameof(ProjectName))]
		private string ValidateProjectName(object value)
		{
			var validator = new Validation.FileNameValidation();
			var validationResult = validator.Validate(value, null);
			return validationResult.IsValid ? null : "Project name contains invalid characters";
		}

		[ValidationFor(nameof(SaveLocation))]
		private string ValidateSaveLocation(object value)
		{
			var validator = new Validation.FolderPathValidation();
			var validationResult = validator.Validate(value, null);

			if(validationResult.IsValid == false)
				return "Save location contains invalid characters";

			if (Path.IsPathRooted(SaveLocation) == false)
				return "Save location must be a complete path";

			return null;
		}		
	}
}

using System.Windows.Input;
using cgwo.Mvvm;

namespace cgwo.ViewModels
{
	public class HomePageViewModel : ViewModel
	{
		private ViewModel _childViewModel;
		public ICommand NewProject => new DelegateCommand(() =>
		{
			_childViewModel = new NewProjectViewModel(Core.ProjectManager.Instance);
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
					Core.ProjectManager.Instance.LoadProject(dlg.FileName);
				}
			}
		});

		public ViewModel ChildViewModel => _childViewModel;
	}
}

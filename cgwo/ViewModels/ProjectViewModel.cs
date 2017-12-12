using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace cgwo.ViewModels
{
	public class ProjectViewModel : Mvvm.ViewModel
	{
		private readonly Core.Project _project;
		private Data.CardTypeViewModel _cardType;
		
		public ProjectViewModel(Core.Project project)
		{
			_project = project ?? throw new ArgumentNullException(nameof(project));
		}

		public string Title => $"Project > {_project.Name}";

		public IEnumerable<object> CardTypes => _project.CardTypes.Select(x => x);

		public Data.CardTypeViewModel CardType => _cardType;
		

		public ICommand AddType => new Mvvm.DelegateCommand(() =>
		{
			var newType = new Core.CardType();
			_cardType = new Data.CardTypeViewModel(newType);
			
			RaisePropertyChanged(nameof(CardType));
		});

		public ICommand Save => new Mvvm.DelegateCommand(() =>
		{
			_cardType.Save();
			RaisePropertyChanged(nameof(CardTypes));
		});
	}
}

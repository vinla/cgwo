using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cgwo.ViewModels
{
	public class CardTypesViewModel : Mvvm.ViewModel
	{
		private readonly Core.ProjectManager _projectManager;

		public CardTypesViewModel(Core.ProjectManager projectManager)
		{
			_projectManager = projectManager ?? throw new ArgumentNullException(nameof(projectManager));
		}
		public string Title => "Card types";

		public IEnumerable<Core.CardType> CardTypes => _projectManager.LoadedProject.CardTypes;
	}
}

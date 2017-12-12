using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cgwo.ViewModels.Data
{
	public class CardTypeViewModel : Mvvm.ViewModel
	{
		private readonly Core.Project _project;
		private readonly Core.CardType _original;
		private readonly Core.CardType _clone;

		public CardTypeViewModel(Core.CardType original)
		{
			_original = original;
			_clone = original.Clone();
		}

		public string Name
		{
			get { return _clone.Name; }
			set
			{
				_clone.Name = value;
				RaisePropertyChanged(nameof(Name));
				RaisePropertyChanged(nameof(HasChanges));
			}
		}

		public bool HasChanges
		{
			get
			{
				return _clone.Name != _original.Name;
			}
		}

		public void Save()
		{
			_clone.Imprint(_original);
			if (Core.ProjectManager.Instance.LoadedProject.CardTypes.Any(ct => ct.Id == _original.Id) == false)
				Core.ProjectManager.Instance.LoadedProject.CardTypes.Add(_original);
			RaisePropertyChanged(nameof(HasChanges));
		}
	}
}

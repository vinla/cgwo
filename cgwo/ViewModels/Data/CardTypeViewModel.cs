using System;
using System.Linq;

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
				NotifyChanges();
			}
		}

		public bool HasChanges => _clone.Name != _original.Name;			

		public bool CardTypeAlreadyExists
		{
			get
			{
				return Core.ProjectManager.Instance.LoadedProject.CardTypes.Any(ct => ct.Name == Name && ct.Id != _original.Id);
			}
		}

		public bool CanSave => HasChanges && !CardTypeAlreadyExists;

		public void Save()
		{
			if (!CanSave)
				throw new InvalidOperationException("Save called when saving is not allowed");

			_clone.Imprint(_original);
			if (Core.ProjectManager.Instance.LoadedProject.CardTypes.Any(ct => ct.Id == _original.Id) == false)
				Core.ProjectManager.Instance.LoadedProject.CardTypes.Add(_original);

			NotifyChanges();
			Core.ProjectManager.Instance.SaveProject();
		}

		private void NotifyChanges()
		{
			RaisePropertyChanged(nameof(HasChanges));
			RaisePropertyChanged(nameof(CardTypeAlreadyExists));
			RaisePropertyChanged(nameof(CanSave));
		}
	}
}

using System;
using System.Linq;
using Cogs.Common;

namespace cgwo.ViewModels.Data
{
	public class CardTypeViewModel : Mvvm.ViewModel
	{
        private readonly ICardGameDataStore _cardGameDataStore;
		private readonly CardType _original;
		private readonly CardType _clone;
        
        public CardTypeViewModel(ICardGameDataStore cardGameDataStore, CardType original)
        {
            _cardGameDataStore = cardGameDataStore ?? throw new ArgumentNullException(nameof(cardGameDataStore));
            _original = original;
            _clone = new CardType
            {
                Id = _original.Id,
                Name = _original.Name
            };
        }

		public Guid Id => _original.Id;

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
				return _cardGameDataStore.GetCardTypes().Any(ct => ct.Name == Name && ct.Id != _original.Id);
			}
		}

		public bool CanSave => HasChanges && !CardTypeAlreadyExists && !String.IsNullOrEmpty(Name);

		public void Save()
		{
			if (!CanSave)
				throw new InvalidOperationException("Save called when saving is not allowed");

            _cardGameDataStore.SaveCardType(_clone);
            _original.Name = _clone.Name;

			NotifyChanges();			
		}

		private void NotifyChanges()
		{
			RaisePropertyChanged(nameof(HasChanges));
			RaisePropertyChanged(nameof(CardTypeAlreadyExists));
			RaisePropertyChanged(nameof(CanSave));
		}
	}
}

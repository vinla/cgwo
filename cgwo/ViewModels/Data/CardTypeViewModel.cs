using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using cgwo.Mvvm;
using Cogs.Common;

namespace cgwo.ViewModels.Data
{
	public class CardTypeViewModel : Mvvm.ViewModel
	{
        private readonly ICardGameDataStore _cardGameDataStore;
        private readonly IDialogService _dialogService;
		private readonly CardType _original;
		private readonly CardType _clone;        
        private readonly Action<bool> _updateCallback;
        private readonly LayoutViewModel _layoutViewModel;
        private List<CardAttributeViewModel> _attributes;

        public CardTypeViewModel(ICardGameDataStore cardGameDataStore, IDialogService dialogService, CardType original, Action<bool> updateCallback)
        {
            _dialogService = dialogService;
            _updateCallback = updateCallback;
            _cardGameDataStore = cardGameDataStore ?? throw new ArgumentNullException(nameof(cardGameDataStore));            
            _original = original;
            _layoutViewModel = new LayoutViewModel();
            LoadAttributes();

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
			}
		}

        [Mvvm.CalculateFrom(nameof(IsCreated))]
        public LayoutViewModel Layout => IsCreated ? _layoutViewModel : null;

        [Mvvm.CalculateFrom(nameof(Name))]
		public bool HasChanges => _clone.Name != _original.Name;			

        [Mvvm.CalculateFrom(nameof(Name))]
		public bool CardTypeAlreadyExists => _cardGameDataStore.GetCardTypes().Any(ct => ct.Name == Name.Trim() && ct.Id != _original.Id);

        [Mvvm.CalculateFrom(nameof(HasChanges))]
		public bool CanSave => HasChanges && !CardTypeAlreadyExists && !String.IsNullOrEmpty(Name);

        public bool IsCreated => _cardGameDataStore.GetCardTypes().Any(ct => ct.Id == _original.Id);

        public ICommand SaveCommand => new Mvvm.DelegateCommand(() =>
        {
            if (!CanSave)
                return;

            Name = Name.Trim();

            _cardGameDataStore.SaveCardType(_clone);
            _original.Name = Name;            
            _updateCallback?.Invoke(false);

            NotifyChanges();
        });

        public ICommand CancelCommand => new Mvvm.DelegateCommand(() =>
        {
            Name = _original.Name;
            if (_cardGameDataStore.GetCardTypes().Any(ct => ct.Id == _original.Id) == false)
                _updateCallback?.Invoke(true);
        });

		private void NotifyChanges()
		{
			RaisePropertyChanged(nameof(HasChanges));
			RaisePropertyChanged(nameof(CardTypeAlreadyExists));
			RaisePropertyChanged(nameof(CanSave));
            RaisePropertyChanged(nameof(IsCreated));
		}

        public IEnumerable<CardAttributeViewModel> Attributes => _attributes.Select(x => x);

        public ICommand AddAttribute => new Mvvm.DelegateCommand(() =>
        {
            var cardAttribute = new CardAttribute();
            _attributes.Add(new CardAttributeViewModel(_cardGameDataStore, _dialogService, _original.Id, cardAttribute, CancelAddAttribute, () => RaisePropertyChanged(nameof(IsEditingAttribute))));
            RaisePropertyChanged(nameof(Attributes));
        });

        [Mvvm.CalculateFrom(nameof(Attributes))]
        public bool IsEditingAttribute => _attributes.Any(attr => attr.HasChanges);

        private void LoadAttributes()
        {
            _attributes = _cardGameDataStore.GetCardAttributes(_original.Id).Select(ca => new CardAttributeViewModel(_cardGameDataStore, _dialogService, _original.Id, ca, RemoveDeleted, () => RaisePropertyChanged(nameof(IsEditingAttribute)))).ToList();
        }

        private void CancelAddAttribute()
        {
            _attributes.RemoveAll(attr => attr.IsNew);
            RaisePropertyChanged(nameof(Attributes));
        }

        private void RemoveDeleted()
        {
            _attributes.RemoveAll(attr => attr.IsDeleted);
            RaisePropertyChanged(nameof(Attributes));
        }
    }
}

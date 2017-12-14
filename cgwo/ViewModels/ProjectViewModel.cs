using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using cgwo.Mvvm;
using Cogs.Common;

namespace cgwo.ViewModels
{
	public class ProjectViewModel : ViewModel
	{
        private ICardGameDataStore _cardGameDataStore;
        private readonly string _projectName;
        private readonly IDialogService _dialogService; 
				
		public ProjectViewModel(ICardGameDataStore cardGameDataStore, IDialogService dialogService)
		{
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _cardGameDataStore = cardGameDataStore ?? throw new ArgumentNullException(nameof(cardGameDataStore));
            _projectName = _cardGameDataStore.GetProjectInfo().Name;
		}

		public string Title => $"Project > {_projectName}";

		public IEnumerable<object> CardTypes => _cardGameDataStore.GetCardTypes().Select(x => x);

		public Data.CardTypeViewModel CardType
        {
            get { return GetValue<Data.CardTypeViewModel>(nameof(CardType)); }
            private set { SetValue(nameof(CardType), value); }
        }

		public CardType SelectedCardType
		{
			get { return GetValue<CardType>(nameof(SelectedCardType)); }
			set
			{
				if (CardType != null && value != null)
				{
					if (CardType.Id == value.Id)
						return;

					if (CardType.HasChanges)
					{
                        if (_dialogService.Prompt("Unsaved changes", "Do you want to swap and lose unsaved changes?") == DialogResult.Reject)
                            return;						
					}
				}

				SetValue(nameof(SelectedCardType), value);                

				if (value != null)
				{
					CardType = new Data.CardTypeViewModel(_cardGameDataStore, SelectedCardType);
				}
			}
		}

        [Mvvm.CalculateFrom(nameof(SelectedCardType))]
        [Mvvm.CalculateFrom(nameof(CardType))]
        public bool CanDelete => SelectedCardType != null && CardType != null && SelectedCardType.Id == CardType.Id;

		public ICommand AddType => new Mvvm.DelegateCommand(() =>
		{
			var newType = new CardType
			{
				Name = String.Empty
			};

			CardType = new Data.CardTypeViewModel(_cardGameDataStore, newType);
            SelectedCardType = null;
		});

        public ICommand DeleteType => new Mvvm.DelegateCommand(() =>
        {
            if (SelectedCardType != null)
            {
                if (_dialogService.Prompt("Confirm deletion", "Are you sure you want to delete this card?") == DialogResult.Accept)
                {                    
                    _cardGameDataStore.DeleteCardType(SelectedCardType);
                    RaisePropertyChanged(nameof(CardTypes));
                    CardType = null;
                }
            }
        });

		public ICommand Save => new Mvvm.DelegateCommand(() =>
		{
            if (CardType != null)
            {
                CardType.Save();
                RaisePropertyChanged(nameof(CardTypes));
            }
		});
	}
}

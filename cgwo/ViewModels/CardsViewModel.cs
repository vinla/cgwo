using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cogs.Common;
using GorgleDevs.Wpf.Mvvm;

namespace cgwo.ViewModels
{
    public class CardsViewModel : ModuleViewModel
    {
        private readonly ICardGameDataStore _cardGameDataStore;
        private readonly IDialogService _dialogService;
        private readonly IEnumerable<CardType> _cardTypes;
		private List<Card> _cards;

        public CardsViewModel(ICardGameDataStore cardGameDataStore, IDialogService dialogService)
        {
            _cardGameDataStore = cardGameDataStore ?? throw new ArgumentNullException(nameof(cardGameDataStore));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(cardGameDataStore));
            _cardTypes = _cardGameDataStore.GetCardTypes();			
        }

        public IEnumerable<CardType> CardTypes => _cardTypes.Select(x => x);
        
        public override bool BeforeUnload()
        {
            return true;
        }

		public IEnumerable<Card> Cards => _cards ?? (_cards = _cardGameDataStore.GetCards().ToList());

        public ICommand AddCard => new DelegateCommand((o) =>
        {
            var cardType = (CardType)o;            
            var attributes = _cardGameDataStore.GetCardAttributes(cardType.Id);

            var card = new Card(cardType, attributes.Select(attr => new CardAttributeValue(attr)));
			CardEditor = new Data.CardEditorViewModel(_cardGameDataStore, _dialogService, card, AfterCardUpdate);
        });        

        public Data.CardEditorViewModel CardEditor
        {
            get { return GetValue<Data.CardEditorViewModel>(nameof(CardEditor)); }
            private set { SetValue(nameof(CardEditor), value); }
        }

        public CardType SelectedCardType
        {
            get { return GetValue<CardType>(nameof(SelectedCardType)); }
            set { SetValue(nameof(SelectedCardType), value); }
        }

        public Card SelectedCard
        {
            get { return GetValue<Card>(nameof(SelectedCard)); }
            set
            {
                SetValue(nameof(SelectedCard), value);
				if (SelectedCard != null)
				{
					CardEditor = new Data.CardEditorViewModel(_cardGameDataStore, _dialogService, SelectedCard, AfterCardUpdate);
					CardEditor.UpdatePreview.Execute(null);
				}
				else
					CardEditor = null;
            }
        }

		private void AfterCardUpdate(Card card)
		{
			_cards = _cardGameDataStore.GetCards().ToList();
			RaisePropertyChanged(nameof(Cards));
			SelectedCard = card != null ? _cards.Single(c => c.Id == card.Id) : (Card)null;			
		}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using cgwo.Mvvm;
using Cogs.Common;

namespace cgwo.ViewModels
{
    public class CardsViewModel : ModuleViewModel
    {
        private readonly ICardGameDataStore _cardGameDataStore;
        private readonly IDialogService _dialogService;
        private readonly IEnumerable<CardType> _cardTypes;

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

        public IEnumerable<Card> Cards => _cardGameDataStore.GetCards();

        public ICommand AddCard => new Mvvm.DelegateCommand((o) =>
        {
            var cardType = (CardType)o;            
            var attributes = _cardGameDataStore.GetCardAttributes(cardType.Id);

            var card = new Card(cardType, attributes.Select(attr => new CardAttributeValue(attr)));
            CardEditor = new Data.CardEditorViewModel(_cardGameDataStore, _dialogService, card);
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
                CardEditor = new Data.CardEditorViewModel(_cardGameDataStore, _dialogService, SelectedCard);
                CardEditor.UpdatePreview.Execute(null);
            }
        }
    }
}

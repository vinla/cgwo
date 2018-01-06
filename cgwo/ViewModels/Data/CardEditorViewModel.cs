using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using cgwo.Mvvm;
using Cogs.Common;
using Cogs.Designer;
using Cogs.Mvvm;

namespace cgwo.ViewModels.Data
{
    public class CardEditorViewModel : Mvvm.ViewModel
    {
        private readonly ICardGameDataStore _cardGameDataStore;
        private readonly IDialogService _dialogService;
        private readonly Card _card;
        private readonly CardLayout _layout;
		private readonly Action _onCardSaved;
        private IEnumerable<CardElement> _elements;

        public CardEditorViewModel(ICardGameDataStore cardGameDataStore, IDialogService dialogService, Card card, Action onCardSaved)
        {
            _cardGameDataStore = cardGameDataStore ?? throw new ArgumentNullException(nameof(cardGameDataStore));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
			_onCardSaved = onCardSaved;
            _card = card ?? throw new ArgumentNullException(nameof(card));
            _layout = _cardGameDataStore.GetLayout(card.CardType.Id);
        }

        public string Name
        {
            get { return _card.Name; }
            set
            {
                _card.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public IEnumerable<CardAttributeValue> AttributeValues => _card.AttributeValues;

        public ICommand UpdatePreview => new Mvvm.DelegateCommand(() =>
        {
            _elements = LayoutConverter.ToDesignerElements(_layout.Elements);
            foreach(var textElement in _elements.OfType<TextElement>())
            {
                var matches = Regex.Matches(textElement.Text, "{([A-Za-z0-9]+)}");
                foreach(var match in matches.Cast<Match>().Where(m => m.Success))
                {
                    if (match.Groups[1].Value == "Name")
                        textElement.Text = textElement.Text.Replace(match.Value, _card.Name);
                    else
                    {
                        var attribute = _card.AttributeValues.SingleOrDefault(attr => attr.CardAttribute.Name == match.Groups[1].Value);
                        if (attribute != null)
                        {
                            textElement.Text = textElement.Text.Replace(match.Value, attribute.Value);
                        }
                    }
                }
            }
            RaisePropertyChanged(nameof(Elements));
            RaisePropertyChanged(nameof(BackgroundColor));
        });        

        public Brush BackgroundColor => new SolidColorBrush(ColorExtensionMethods.FromHex(_layout.BackgroundColor));

        public IEnumerable<CardElement> Elements => _elements;

        public ICommand SaveCard => new DelegateCommand(() =>
        {
            _cardGameDataStore.SaveCard(_card);
			_onCardSaved?.Invoke();
        });
    }
}

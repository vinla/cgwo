using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;
using Cogs.Common;
using Cogs.Designer;
using GorgleDevs.Wpf;
using GorgleDevs.Wpf.Mvvm;

namespace cgwo.ViewModels.Data
{
    public class CardEditorViewModel : ViewModel
    {
        private readonly ICardGameDataStore _cardGameDataStore;
        private readonly IDialogService _dialogService;
        private readonly Card _card;
        private readonly CardLayout _layout;
		private readonly Action<Card> _cardUpdated;		
        private IEnumerable<CardElement> _elements;

        public CardEditorViewModel(ICardGameDataStore cardGameDataStore, IDialogService dialogService, Card card, Action<Card> cardUpdated)
        {
            _cardGameDataStore = cardGameDataStore ?? throw new ArgumentNullException(nameof(cardGameDataStore));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
			_cardUpdated = cardUpdated;
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

        public ICommand UpdatePreview => new DelegateCommand(() =>
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
            
            foreach(var imageElement in _elements.OfType<ImageElement>().Where(img => img.ImageSource == "Card Attribute"))
            {
                var attributeValue = _card.AttributeValues.SingleOrDefault(av => av.CardAttribute.Name == imageElement.LinkedAttribute);
                if (attributeValue != null)
                {
                    imageElement.ImageData = attributeValue.Value;
                }
            }

            RaisePropertyChanged(nameof(Elements));
            RaisePropertyChanged(nameof(BackgroundColor));
        });        

        public Brush BackgroundColor => new SolidColorBrush(ColorExtensionMethods.FromHex(_layout.BackgroundColor));

        public IEnumerable<CardElement> Elements => _elements;

        public ICommand SaveCard => new DelegateCommand(() =>
        {
            _card.ImageData = CardImageComposer.CreateCardImageBase64(_card, _layout);
            _cardGameDataStore.SaveCard(_card);
			_cardUpdated?.Invoke(_card);
        });

		public ICommand DeleteCard => new DelegateCommand(() =>
		{
			if (_dialogService.Prompt("Confirm delete", "Are you sure you want to delete this card?") == DialogResult.Accept)
			{
				_cardGameDataStore.DeleteCard(_card);
				_cardUpdated?.Invoke(null);
			}
		});
    }
}

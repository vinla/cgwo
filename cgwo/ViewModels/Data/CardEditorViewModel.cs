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
		private readonly List<NamedValueViewModel> _cardValues;
        private readonly CardLayout _layout;
		private readonly Action<Card> _cardUpdated;		
        private IEnumerable<CardElement> _elements;

        public CardEditorViewModel(ICardGameDataStore cardGameDataStore, IDialogService dialogService, Card card, Action<Card> cardUpdated)
        {
            _cardGameDataStore = cardGameDataStore ?? throw new ArgumentNullException(nameof(cardGameDataStore));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
			_cardUpdated = cardUpdated;
            _card = card ?? throw new ArgumentNullException(nameof(card));

			_cardValues = _card.AttributeValues.Select(attr => new CardAttributeValueViewModel(attr)).Cast<NamedValueViewModel>().ToList();
			_cardValues.Insert(0, new CardNameValueViewModel(_card));
			_cardValues.Insert(1, new CardTypeValueViewModel(_card));

			_layout = _cardGameDataStore.GetLayout(card.CardType.Id);
			_elements = LayoutConverter.ToDesignerElements(_layout.Elements);
			
			foreach(var cardValue in _cardValues)
			{
				cardValue.PropertyChanged += (s, e) =>
				{
					if(e.PropertyName == nameof(NamedValueViewModel.Value))
					{
						UpdatePreview(cardValue);
					}
				};
			}
		}

        public IEnumerable<NamedValueViewModel> Values => _cardValues.Where(cv => String.IsNullOrEmpty(cv.Editor) == false);
        
		public Brush Background
		{
			get
			{
				if (_layout.BackgroundImage != null && _layout.BackgroundImage.Length > 0)
				{
					return new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(_layout.BackgroundImage));
				}

				return new SolidColorBrush(ColorExtensionMethods.FromHex(_layout.BackgroundColor));
			}
		}

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

		public void UpdateTextItems()
		{
			foreach (var element in _elements.OfType<TextElement>())
			{
				var text = element.BaseText;
				foreach (var cv in _cardValues)
				{
					text = text.Replace("{" + cv.Name + "}", cv.ValueOrDefault);
				}
				element.Text = text;
			}
		}

		public void UpdatePreview()
		{
			UpdateTextItems();
			foreach (var image in _elements.OfType<ImageElement>().Where(el => el.ImageSource == "Card Attribute"))
			{
				var valueProvider = _cardValues.SingleOrDefault(cv => cv.Name == image.LinkedAttribute);
				if (valueProvider != null && !String.IsNullOrEmpty(valueProvider.Value))
					image.ImageData = Convert.FromBase64String(valueProvider.Value);
			}
		}

		public void UpdatePreview(NamedValueViewModel changed)
		{
			if (changed.Editor == "TextEditor")
			{
				UpdateTextItems();
			}
			else if(changed.Editor == "ImageEditor")
			{
				foreach(var image in _elements.OfType<ImageElement>().Where(el => el.ImageSource == "Card Attribute" && el.LinkedAttribute == changed.Name))
				{
					image.ImageData = Convert.FromBase64String(changed.Value);
				}
			}
		}
    }	
}

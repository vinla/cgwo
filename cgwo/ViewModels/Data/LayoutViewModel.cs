using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using Cogs.Designer;
using Cogs.Common;
using GorgleDevs.Wpf;
using GorgleDevs.Wpf.Mvvm;
using Cogs.Designer.Actions;

namespace cgwo.ViewModels.Data
{
    public class LayoutViewModel : ViewModel, IZIndexManager
    {
        private readonly IDialogService _dialogService;
        private readonly ICardGameDataStore _cardGameDataStore;
        private readonly CardTypeViewModel _cardTypeViewModel;
		private readonly ActionManager _actionManager;
		private readonly LayoutDocument _layoutDocument;

        public LayoutViewModel(ICardGameDataStore cardGameDataStore, IDialogService dialogService, CardTypeViewModel cardTypeViewModel)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _cardGameDataStore = cardGameDataStore ?? throw new ArgumentNullException(nameof(cardGameDataStore));
            _cardTypeViewModel = cardTypeViewModel ?? throw new ArgumentNullException(nameof(cardTypeViewModel));
			_layoutDocument = new LayoutDocument();
			_actionManager = new ActionManager();
            LoadLayout();

			_layoutDocument.Elements.CollectionChanged += (s, e) => RaisePropertyChanged(nameof(SelectedElement));
			
        }

		public LayoutDocument LayoutDocument => _layoutDocument;

		public CardElement SelectedElement => _layoutDocument.Elements.Count(x => x.Selected) == 1 ? _layoutDocument.Elements.First(x => x.Selected) : null;

		public ActionManager ActionManager => _actionManager;

        public ICommand AddElement => new DelegateCommand((o) =>
        {
            var element = o as CardElement;
            
            if(element != null)
            {
				element.PropertyChanged += (s, e) =>
				{
					if(e.PropertyName == nameof(element.Selected))
					{
						RaisePropertyChanged(nameof(SelectedElement));
					}
				};

				var addElementAction = new AddElementAction(_layoutDocument, element);
				addElementAction.Redo();
				_actionManager.Push(addElementAction);
                
                element.ZIndexManager = this;
                BringToFront(element);                
				SelectElement(element);
            }
        });

        public ICommand DeleteCommand => new DelegateCommand(() =>
        {
			var deleteAction = new DeleteElementsAction(_layoutDocument, _layoutDocument.Elements.Where(el => el.Selected));
			deleteAction.Redo();
			_actionManager.Push(deleteAction);			
			RaisePropertyChanged(nameof(SelectedElement));
        });

        public ICommand ReloadCommand => new DelegateCommand(() =>
        {
			if (_dialogService.Prompt("Reload layout", "Do you want to reload the layout? This will revert all changes made since your last save.") == DialogResult.Accept)
			{				
				LoadLayout();
				_actionManager.Reset();
				RaisePropertyChanged(nameof(LayoutDocument));
			}
        });

		public ICommand UndoCommand => new DelegateCommand(() =>
		{
			_actionManager.Undo();
		});

		public ICommand RedoCommand => new DelegateCommand(() =>
		{
			_actionManager.Redo();
		});

        public ICommand SelectImage => new DelegateCommand((o) =>
        {
            var imageElement = o as ImageElement;
            var (result, path) = _dialogService.ChooseFile(String.Empty, "Images files|*.png;*.bmp;*.jpg;*.jpeg;*.gif");
            if(result == DialogResult.Accept)
            {
                imageElement.ImageSource = Convert.ToBase64String(System.IO.File.ReadAllBytes(path));
            }
        });

		public ICommand BringFowardsCommand => new DelegateCommand(() =>
		{
			foreach (var element in _layoutDocument.Elements.Where(el => el.Selected))
				BringForwards(element);
		});

		public ICommand BringToFrontCommand => new DelegateCommand(() =>
		{
			foreach (var element in _layoutDocument.Elements.Where(el => el.Selected))
				BringToFront(element);
		});

		public ICommand SendBackwardsCommand => new DelegateCommand(() =>
		{
			foreach (var element in _layoutDocument.Elements.Where(el => el.Selected))
				SendBackwards(element);
		});

		public ICommand SendToBackCommand => new DelegateCommand(() =>
		{
			foreach (var element in _layoutDocument.Elements.Where(el => el.Selected))
				SendToBack(element);
		});

        public void BringForwards(CardElement element)
        {
            var currentZIndex = element.ZIndex;
            var target = _layoutDocument.Elements.Where(t => t.ZIndex > currentZIndex).OrderBy(t => t.ZIndex).FirstOrDefault();
            if(target != null)
            {
                element.ZIndex = target.ZIndex;
                target.ZIndex = currentZIndex;
            }                
        }

        public void BringToFront(CardElement element)
        {
            element.ZIndex = _layoutDocument.Elements.Max(e => e.ZIndex) + 1;
        }

        public void SendBackwards(CardElement element)
        {
            var currentZIndex = element.ZIndex;
            var target = _layoutDocument.Elements.Where(t => t.ZIndex > currentZIndex).OrderBy(t => t.ZIndex).FirstOrDefault();
            if (target != null)
            {
                element.ZIndex = target.ZIndex;
                target.ZIndex = currentZIndex;
            }
        }

        public void SendToBack(CardElement element)
        {
            element.ZIndex = _layoutDocument.Elements.Min(e => e.ZIndex) - 1;
        }

        public ICommand SaveLayoutCommand => new DelegateCommand(() =>
        {
            SaveLayout();
        });        

        private void LoadLayout()
        {
            var layout = _cardGameDataStore.GetLayout(_cardTypeViewModel.Id);

            if(layout != null)
            {
                _layoutDocument.BackgroundColor = (Color)ColorConverter.ConvertFromString(layout.BackgroundColor);
                _layoutDocument.BackgroundImage = Convert.FromBase64String(layout.BackgroundImage ?? String.Empty);
				var elements = LayoutConverter.ToDesignerElements(layout.Elements);
				foreach (var element in elements)
				{
					element.PropertyChanged += (s, e) =>
					{
						if (e.PropertyName == nameof(element.Selected))
						{
							RaisePropertyChanged(nameof(SelectedElement));
						}
					};
					_layoutDocument.Elements.Add(element);
				}
            }
        }

        private void SaveLayout()
        {
            var layout = new CardLayout
            {
                BackgroundColor = _layoutDocument.BackgroundColor.ToHex(),
                BackgroundImage = Convert.ToBase64String(_layoutDocument.BackgroundImage ?? new byte[0])
            };

            foreach(var imageElement in _layoutDocument.Elements.OfType<ImageElement>())
            {
                if (imageElement.ImageSource == "Image")
                    imageElement.LinkedAttribute = String.Empty;
                else if (imageElement.ImageSource == "Card Attribute")
                    imageElement.ImageData = String.Empty;
            }
                

            layout.Elements.AddRange(LayoutConverter.FromDesignerElements(_layoutDocument.Elements));

            _cardGameDataStore.SaveLayout(_cardTypeViewModel.Id, layout);
            _cardGameDataStore.UpdateCardTypeImage(_cardTypeViewModel.Id, CardImageComposer.CreateCardImage(layout));
        }        

		private void SelectElement(CardElement element)
		{
			foreach (var el in _layoutDocument.Elements)
				el.Selected = el == element;
		}

        public IEnumerable<String> ImageAttributes => _cardTypeViewModel.Attributes.Where(attr => attr.Type == AttributeType.Image).Select(attr => attr.Name);        
    }    
}

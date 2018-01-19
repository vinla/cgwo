using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using Cogs.Designer;
using Cogs.Common;
using GorgleDevs.Wpf;
using GorgleDevs.Wpf.Mvvm;


namespace cgwo.ViewModels.Data
{
    public class LayoutViewModel : ViewModel, IZIndexManager
    {
        private readonly IDialogService _dialogService;
        private readonly ICardGameDataStore _cardGameDataStore;
        private readonly CardTypeViewModel _cardTypeViewModel;
        private List<CardElement> _elements = new List<CardElement>();

        public LayoutViewModel(ICardGameDataStore cardGameDataStore, IDialogService dialogService, CardTypeViewModel cardTypeViewModel)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _cardGameDataStore = cardGameDataStore ?? throw new ArgumentNullException(nameof(cardGameDataStore));
            _cardTypeViewModel = cardTypeViewModel ?? throw new ArgumentNullException(nameof(cardTypeViewModel));
            LoadLayout();
        }

        public Color BackgroundColor
        {
            get { return GetValue<Color>(nameof(BackgroundColor)); }
            set { SetValue(nameof(BackgroundColor), value); }
        }

        public byte[] BackgroundImage
        {
            get { return GetValue<byte[]>(nameof(BackgroundImage)); }
            set { SetValue(nameof(BackgroundImage), value); }
        }

        [CalculateFrom(nameof(BackgroundColor))]
        [CalculateFrom(nameof(BackgroundImage))]
        public Brush Background
        {
            get
            {
                if(BackgroundImage != null && BackgroundImage.Length > 0)
                {
                    return new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(BackgroundImage));
                }

                return new SolidColorBrush(BackgroundColor);
            }
        }

        public IEnumerable<CardElement> Elements => _elements.Select(x => x);

        public CardElement SelectedElement
        {
            get { return GetValue<CardElement>(nameof(SelectedElement)); }
            set { SetValue(nameof(SelectedElement), value); }
        }

        public ICommand AddElement => new DelegateCommand((o) =>
        {
            var element = o as CardElement;
            
            if(element != null)
            {
                _elements.Add(element);
                element.ZIndexManager = this;
                BringToFront(element);
                RaisePropertyChanged(nameof(Elements));
                SelectedElement = element;
            }
        });

        public ICommand DeleteCommand => new DelegateCommand(() =>
        {
            if (SelectedElement != null)
            {
                _elements.Remove(SelectedElement);
                RaisePropertyChanged(nameof(Elements));
                SelectedElement = null;
            }
        });
        public ICommand ReloadCommand => new DelegateCommand(() =>
        {
            LoadLayout();
            RaisePropertyChanged(nameof(Background));
            RaisePropertyChanged(nameof(Elements));
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

        public void BringForwards(CardElement element)
        {
            var currentZIndex = element.ZIndex;
            var target = _elements.Where(t => t.ZIndex > currentZIndex).OrderBy(t => t.ZIndex).FirstOrDefault();
            if(target != null)
            {
                element.ZIndex = target.ZIndex;
                target.ZIndex = currentZIndex;
            }                
        }

        public void BringToFront(CardElement element)
        {
            element.ZIndex = _elements.Max(e => e.ZIndex) + 1;
        }

        public void SendBackwards(CardElement element)
        {
            var currentZIndex = element.ZIndex;
            var target = _elements.Where(t => t.ZIndex > currentZIndex).OrderBy(t => t.ZIndex).FirstOrDefault();
            if (target != null)
            {
                element.ZIndex = target.ZIndex;
                target.ZIndex = currentZIndex;
            }
        }

        public void SendToBack(CardElement element)
        {
            element.ZIndex = _elements.Min(e => e.ZIndex) - 1;
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
                BackgroundColor = (Color)ColorConverter.ConvertFromString(layout.BackgroundColor);
                BackgroundImage = Convert.FromBase64String(layout.BackgroundImage ?? String.Empty);
                _elements.AddRange(LayoutConverter.ToDesignerElements(layout.Elements));                
            }
        }

        private void SaveLayout()
        {
            var layout = new CardLayout
            {
                BackgroundColor = BackgroundColor.ToHex(),
                BackgroundImage = Convert.ToBase64String(BackgroundImage ?? new byte[0])
            };

            foreach(var imageElement in Elements.OfType<ImageElement>())
            {
                if (imageElement.ImageSource == "Image")
                    imageElement.LinkedAttribute = String.Empty;
                else if (imageElement.ImageSource == "Card Attribute")
                    imageElement.ImageData = String.Empty;
            }
                

            layout.Elements.AddRange(LayoutConverter.FromDesignerElements(Elements));

            _cardGameDataStore.SaveLayout(_cardTypeViewModel.Id, layout);
            _cardGameDataStore.UpdateCardTypeImage(_cardTypeViewModel.Id, CardImageComposer.CreateCardImage(layout));
        }        

        public IEnumerable<String> ImageAttributes => _cardTypeViewModel.Attributes.Where(attr => attr.Type == AttributeType.Image).Select(attr => attr.Name);        
    }    
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using Cogs.Designer;
using Cogs.Common;
using Cogs.Mvvm;

namespace cgwo.ViewModels.Data
{
    public class LayoutViewModel : Mvvm.ViewModel, IZIndexManager
    {
        private readonly ICardGameDataStore _cardGameDataStore;
        private readonly Guid _cardTypeId;
        private List<CardElement> _elements = new List<CardElement>();

        public LayoutViewModel(ICardGameDataStore cardGameDataStore, Guid cardTypeId)
        {
            _cardGameDataStore = cardGameDataStore ?? throw new ArgumentNullException(nameof(cardGameDataStore));
            _cardTypeId = cardTypeId;
            LoadLayout();
        }

        public Color BackgroundColor
        {
            get { return GetValue<Color>(nameof(BackgroundColor)); }
            set { SetValue(nameof(BackgroundColor), value); }
        }

        [Mvvm.CalculateFrom(nameof(BackgroundColor))]
        public Brush Background => new SolidColorBrush(BackgroundColor);
        //public Brush Background => new ImageBrush(new BitmapImage(new System.Uri("pack://application:,,,/Resources/Images/back.jpg", System.UriKind.Absolute)));

        public IEnumerable<CardElement> Elements => _elements.Select(x => x);

        public CardElement SelectedElement
        {
            get { return GetValue<CardElement>(nameof(SelectedElement)); }
            set { SetValue(nameof(SelectedElement), value); }
        }

        public ICommand AddElement => new Mvvm.DelegateCommand((o) =>
        {
            var elementType = o.ToString();
            CardElement elementToAdd = null;
            switch(elementType)
            {
                case "Text":
                    elementToAdd = new TextElement
                    {
                        Left = 10,
                        Top = 10,
                        Height = 24,
                        Width = 100,
                        Text = "New text element",
                        TextColor = Colors.Black,
                        TextSize = 12
                    };
                    break;
                case "Rectangle":
                    elementToAdd = new RectangleElement
                    {
                        Left = 10,
                        Top = 10,
                        Height = 24,
                        Width = 100,
                        BackgroundColor = Colors.White,
                        BorderColor = Colors.Black,
                        BorderWidth = 1
                    };
                    break;
                case "Ellipse":
                    elementToAdd = new EllipseElement
                    {
                        Left = 10,
                        Top = 10,
                        Height = 24,
                        Width = 24,
                        BackgroundColor = Colors.White,
                        BorderColor = Colors.Black,
                        BorderWidth = 1
                    };
                    break;
                default:
                    throw new NotSupportedException("That value is not supported");
            }

            if(elementToAdd != null)
            {
                _elements.Add(elementToAdd);
                elementToAdd.ZIndexManager = this;
                BringToFront(elementToAdd);
                RaisePropertyChanged(nameof(Elements));
                SelectedElement = elementToAdd;
            }
        });

        public ICommand DeleteCommand => new Mvvm.DelegateCommand(() =>
        {
            if (SelectedElement != null)
            {
                _elements.Remove(SelectedElement);
                RaisePropertyChanged(nameof(Elements));
                SelectedElement = null;
            }
        });
        public ICommand ReloadCommand => new Mvvm.DelegateCommand(() =>
        {
            LoadLayout();
            RaisePropertyChanged(nameof(Background));
            RaisePropertyChanged(nameof(Elements));
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

        public ICommand SaveLayoutCommand => new Mvvm.DelegateCommand(() =>
        {
            SaveLayout();
        });        

        private void LoadLayout()
        {
            var layout = _cardGameDataStore.GetLayout(_cardTypeId);

            if(layout != null)
            {
                BackgroundColor = (Color)ColorConverter.ConvertFromString(layout.BackgroundColor);
                foreach(var element in layout.Elements)
                {
                    if (element is TextElementLayout text)
                        _elements.Add(AutoMapper.Mapper.Map<TextElement>(text));
                    if (element is RectangleElementLayout rect)
                        _elements.Add(AutoMapper.Mapper.Map<RectangleElement>(rect));
                    if (element is EllipseElementLayout ellipse)
                        _elements.Add(AutoMapper.Mapper.Map<EllipseElement>(ellipse));
                }
            }
        }

        private void SaveLayout()
        {
            var layout = new CardLayout
            {
                BackgroundColor = BackgroundColor.ToHex()
            };

            foreach(var element in Elements)
            {
                if (element is TextElement text)
                    layout.Elements.Add(AutoMapper.Mapper.Map<TextElementLayout>(text));
                else if (element is RectangleElement rect)
                    layout.Elements.Add(AutoMapper.Mapper.Map<RectangleElementLayout>(rect));
                else if (element is EllipseElement ellipse)
                    layout.Elements.Add(AutoMapper.Mapper.Map<EllipseElementLayout>(ellipse));
            }

            _cardGameDataStore.SaveLayout(_cardTypeId, layout);
        }
    }    
}

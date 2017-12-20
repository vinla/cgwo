using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using Cogs.Designer;

namespace cgwo.ViewModels.Data
{
    public class LayoutViewModel : Mvvm.ViewModel, IZIndexManager
    {
        private List<CardElement> _elements = new List<CardElement>();

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
    }    
}

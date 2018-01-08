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

        [CalculateFrom(nameof(BackgroundColor))]
        public Brush Background => new SolidColorBrush(BackgroundColor);
        //public Brush Background => new ImageBrush(new BitmapImage(new System.Uri("pack://application:,,,/Resources/Images/back.jpg", System.UriKind.Absolute)));

        public IEnumerable<CardElement> Elements => _elements.Select(x => x);

        public CardElement SelectedElement
        {
            get { return GetValue<CardElement>(nameof(SelectedElement)); }
            set { SetValue(nameof(SelectedElement), value); }
        }

        public ICommand AddElement => new DelegateCommand((o) =>
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
            var layout = _cardGameDataStore.GetLayout(_cardTypeId);

            if(layout != null)
            {
                BackgroundColor = (Color)ColorConverter.ConvertFromString(layout.BackgroundColor);
                _elements.AddRange(LayoutConverter.ToDesignerElements(layout.Elements));                
            }
        }

        private void SaveLayout()
        {
            var layout = new CardLayout
            {
                BackgroundColor = BackgroundColor.ToHex()
            };

            layout.Elements.AddRange(LayoutConverter.FromDesignerElements(Elements));

            _cardGameDataStore.SaveLayout(_cardTypeId, layout);
        }

        public ICommand SaveImageCommand => new DelegateCommand(() =>
        {
            SaveCardImage();
        });

        private void SaveCardImage()
        {
            var size = new Size(250, 350);

            var border = new Border();
            border.Width = size.Width;
            border.Height = size.Height;
            border.Background = Background;
            border.BorderThickness = new Thickness(2);
            border.BorderBrush = Brushes.Black;
            border.CornerRadius = new CornerRadius(5);

            var canvas = new Controls.DesignerCanvas();
            canvas.Width = size.Width;
            canvas.Height = size.Height;
            canvas.Elements = Elements;

            canvas.Measure(size);
            canvas.Arrange(new Rect(new Point(0, 0), size));
            canvas.UpdateLayout();

            border.Child = canvas;

            border.Measure(size);
            border.Arrange(new Rect(new Point(0, 0), size));
            border.UpdateLayout();
                        
            var renderTarget = new RenderTargetBitmap(250, 350, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(border);
            renderTarget.Render(canvas);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTarget));

            using (var stream = System.IO.File.OpenWrite(@"c:\utilities\test.png"))
            {
                encoder.Save(stream);
                stream.Flush();
                stream.Close();
            }
        }
    }    
}

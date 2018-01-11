using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using GorgleDevs.Wpf;

namespace Cogs.Designer
{
    public class DesignerElementFactoryConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty TargetElementProperty = DependencyProperty.Register(nameof(TargetElement), typeof(FrameworkElement), typeof(DesignerElementFactoryConverter));
        public FrameworkElement TargetElement
        {
            get; set;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var requestedElementType = value.ToString();
            CardElement element = null;

            switch(requestedElementType)
            {
                case "Text":
                    element = new TextElement
                    {
                        Text = "New text",
                        TextColor = Colors.Black,
                        TextSize = 12
                    };
                    break;
                case "Rectangle":
                    element = new RectangleElement
                    {
                        BackgroundColor = Colors.White,
                        BorderColor = Colors.Black,
                        BorderWidth = 1
                    };
                    break;
                case "Ellipse":
                    element = new EllipseElement
                    {
                        BackgroundColor = Colors.White,
                        BorderColor = Colors.Black,
                        BorderWidth = 1
                    };
                    break;
                case "Image":
                    element = new ImageElement
                    {
                        ImageSource = "Image"
                    };
                    break;
                default:
                    throw new InvalidOperationException("Unknown element type requested");
            }
            
            var offset = parameter != null ? (Point)parameter : new Point(10, 10);
            element.Left = offset.X;
            element.Top = offset.Y;
            element.Width = 120;
            element.Height = 24;

            return element;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("This converter only supports one way converstion");
        }
    }
}

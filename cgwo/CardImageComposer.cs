using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Cogs.Common;
using Cogs.Designer;

namespace cgwo
{
    public static class CardImageComposer
    {
        public static string CreateCardImageBase64(Card card, CardLayout cardLayout)
        {
            return Convert.ToBase64String(CreateCardImage(card, cardLayout));
        }

        public static byte[] CreateCardImage(Card card, CardLayout layout)
        {
            var background = GetBackground(layout);
            var elements = LayoutConverter.ToDesignerElements(layout.Elements);
            CardElementMapper.MapElementValues(card, elements);

            return CreateCardImage(background, elements);
        }

        public static byte[] CreateCardImage(CardLayout layout)
        {
            var background = GetBackground(layout);
            var elements = LayoutConverter.ToDesignerElements(layout.Elements);            

            return CreateCardImage(background, elements);
        }

        private static byte[] CreateCardImage(Brush background, IEnumerable<CardElement> layoutElements)
        {
            var size = new Size(250, 350);            

            var border = new Border();
            border.Width = size.Width;
            border.Height = size.Height;
            border.Background = background;
            border.BorderThickness = new Thickness(2);
            border.BorderBrush = Brushes.Black;
            border.CornerRadius = new CornerRadius(5);

            var canvas = new Controls.DesignerCanvas();
            canvas.Width = size.Width;
            canvas.Height = size.Height;
            canvas.IsDisplayOnly = true;
            canvas.Elements = layoutElements;

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


            using (var stream = new System.IO.MemoryStream())
            {
                encoder.Save(stream);
                stream.Flush();
                stream.Close();
                return stream.GetBuffer();
            }            
        }        

        private static Brush GetBackground(CardLayout layout)
        {
            if (layout.BackgroundImage != null && layout.BackgroundImage.Length > 0)
                return new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(layout.BackgroundImage));
            else if(String.IsNullOrEmpty(layout.BackgroundColor) == false)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(layout.BackgroundColor));

            return null;
        }
    }
}

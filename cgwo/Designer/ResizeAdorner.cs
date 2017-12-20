using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Cogs.Designer
{
    public class ResizeAdorner : Adorner
    {
        private readonly VisualCollection _visuals;

        public ResizeAdorner(UIElement adornedElement) : base(adornedElement)
        {
            _visuals = new VisualCollection(this);
            _visuals.Add(CreateThumb(ThumbPosition.TopLeft));
            _visuals.Add(CreateThumb(ThumbPosition.Top));
            _visuals.Add(CreateThumb(ThumbPosition.TopRight));
            _visuals.Add(CreateThumb(ThumbPosition.Right));
            _visuals.Add(CreateThumb(ThumbPosition.BottomRight));
            _visuals.Add(CreateThumb(ThumbPosition.Bottom));
            _visuals.Add(CreateThumb(ThumbPosition.BottomLeft));
            _visuals.Add(CreateThumb(ThumbPosition.Left));
            _visuals.Add(CreateThumb(ThumbPosition.Center));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var outlinePen = new Pen(Brushes.Blue, 2);
            outlinePen.DashStyle = new DashStyle(new[] { 1d, 1d }, 0);
            var rect = new Rect(1, 1, AdornedElement.RenderSize.Width, RenderSize.Height);
            drawingContext.DrawRectangle(null, outlinePen, rect);

            base.OnRender(drawingContext);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach(var thumb in _visuals.OfType<Thumb>())
            {
                PositionThumb(thumb);                
            }

            return base.ArrangeOverride(finalSize);
        }

        private void PositionThumb(Thumb thumb)
        {
            var thumbSize = new Size(thumb.Width, thumb.Height);
            var midWidth = AdornedElement.DesiredSize.Width / 2f;
            var midHeight = AdornedElement.DesiredSize.Height / 2f;

            var point = new Point();

            switch ((ThumbPosition)thumb.Tag)
            {
                case ThumbPosition.TopLeft:
                    point = new Point(-thumbSize.Width + 1, -thumbSize.Height + 1);
                    break;
                case ThumbPosition.Top:
                    point = new Point(midWidth - (thumbSize.Width / 2f), -thumbSize.Height + 1);
                    break;
                case ThumbPosition.TopRight:
                    point = new Point(AdornedElement.DesiredSize.Width, -thumbSize.Height + 1);
                    break;
                case ThumbPosition.Right:
                    point = new Point(AdornedElement.DesiredSize.Width, midHeight - (thumbSize.Height / 2f));
                    break;
                case ThumbPosition.BottomRight:
                    point = new Point(AdornedElement.DesiredSize.Width, AdornedElement.DesiredSize.Height);
                    break;
                case ThumbPosition.Bottom:
                    point = new Point(midWidth - (thumbSize.Width / 2f), AdornedElement.DesiredSize.Height);
                    break;
                case ThumbPosition.BottomLeft:
                    point = new Point(-thumbSize.Width + 1, AdornedElement.DesiredSize.Height);
                    break;
                case ThumbPosition.Left:
                    point = new Point(-thumbSize.Width, midHeight - (thumbSize.Height / 2f));
                    break;
                case ThumbPosition.Center:
                    point = new Point(midWidth - (thumbSize.Width / 2f), midHeight - (thumbSize.Height / 2f));
                    break;
            }

            thumb.Arrange(new Rect(point, thumbSize));
        }

        private Thumb CreateThumb(ThumbPosition position)
        {
            var thumb = new Thumb();
            thumb.Tag = position;
            thumb.DragDelta += Thumb_DragDelta;
  
            switch(position)
            {
                case ThumbPosition.Top:
                case ThumbPosition.Bottom:
                    thumb.Cursor = Cursors.SizeNS;
                    break;
                case ThumbPosition.Left:
                case ThumbPosition.Right:
                    thumb.Cursor = Cursors.SizeWE;
                    break;
                case ThumbPosition.BottomLeft:
                case ThumbPosition.TopRight:
                    thumb.Cursor = Cursors.SizeNESW;
                    break;
                case ThumbPosition.BottomRight:
                case ThumbPosition.TopLeft:
                    thumb.Cursor = Cursors.SizeNWSE;
                    break;
                case ThumbPosition.Center:
                    thumb.Cursor = Cursors.ScrollAll;
                    break;
            }

            return thumb;
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs args)
        {            
            var thumb = sender as Thumb;
            var thumbPosition = (ThumbPosition)thumb.Tag;
            var adornedElement = (AdornedElement as FrameworkElement).DataContext as CardElement;                       

            if (thumbPosition == ThumbPosition.Right || thumbPosition == ThumbPosition.BottomRight || thumbPosition == ThumbPosition.TopRight)
            {
                //EnforceSize(adornedElement);
                adornedElement.Width = Math.Max(adornedElement.Width + args.HorizontalChange, 1);
            }

            if(thumbPosition == ThumbPosition.Bottom || thumbPosition == ThumbPosition.BottomLeft || thumbPosition == ThumbPosition.BottomRight)
            {
                adornedElement.Height = Math.Max(adornedElement.Height + args.VerticalChange, 1);
            }

            if(thumbPosition == ThumbPosition.Left || thumbPosition == ThumbPosition.BottomLeft || thumbPosition == ThumbPosition.TopLeft)
            {
                double newWidth = Math.Max(adornedElement.Width - args.HorizontalChange, 1);

                adornedElement.Left = adornedElement.Left - (newWidth - adornedElement.Width);
                adornedElement.Width = newWidth;                
            }

            if(thumbPosition == ThumbPosition.Top || thumbPosition == ThumbPosition.TopLeft || thumbPosition == ThumbPosition.TopRight)
            {
                
                double newHeight = Math.Max(adornedElement.Height - args.VerticalChange, 1);

                adornedElement.Top = adornedElement.Top - (newHeight - adornedElement.Height);
                adornedElement.Height = newHeight;                
            }

            if (thumbPosition == ThumbPosition.Center)
            {                
                adornedElement.Top = adornedElement.Top + args.VerticalChange;                
                adornedElement.Left = adornedElement.Left + args.HorizontalChange;
            }
        }

        private void EnforceSize(FrameworkElement adornedElement)
        {
            if (adornedElement.Width.Equals(Double.NaN))
                adornedElement.Width = adornedElement.DesiredSize.Width;
            if (adornedElement.Height.Equals(Double.NaN))
                adornedElement.Height = adornedElement.DesiredSize.Height;

            FrameworkElement parent = adornedElement.Parent as FrameworkElement;
            if (parent != null)
            {
                adornedElement.MaxHeight = parent.ActualHeight;
                adornedElement.MaxWidth = parent.ActualWidth;
            }
        }

        protected override int VisualChildrenCount { get { return _visuals.Count; } }

        protected override Visual GetVisualChild(int index) { return _visuals[index]; }
    }
}

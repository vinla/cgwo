namespace Cogs.Common
{
    public class ShapeElementLayout : ElementLayout
    {
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public int BorderWidth { get; set; }
    }

    public class RectangleElementLayout : ShapeElementLayout
    {

    }

    public class EllipseElementLayout : ShapeElementLayout
    {

    }
}

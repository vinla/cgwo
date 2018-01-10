namespace Cogs.Designer
{
    public class ImageElement : CardElement
    {
        public byte[] ImageSource
        {
            get { return GetValue<byte[]>(nameof(ImageSource)); }
            set { SetValue(nameof(ImageSource), value); }
        }
    }
}

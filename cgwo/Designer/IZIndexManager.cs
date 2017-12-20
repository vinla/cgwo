namespace Cogs.Designer
{
    public interface IZIndexManager
    {
        void BringForwards(CardElement element);
        void SendBackwards(CardElement element);
        void SendToBack(CardElement element);
        void BringToFront(CardElement element);
    }
}

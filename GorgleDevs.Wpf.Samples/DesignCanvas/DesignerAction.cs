namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
	public abstract class DesignerAction
    {
        public bool IsComplete { get; private set; }

        public void SetComplete()
        {
            IsComplete = true;
			AfterCompleted();
        }

		public abstract void Undo();

		public abstract void Redo();

		protected virtual void AfterCompleted()
		{

		}
    }
}

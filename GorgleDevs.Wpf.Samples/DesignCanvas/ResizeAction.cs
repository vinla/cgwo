using System;

namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
	public class ResizeAction : DesignerAction
    {
		private readonly LayoutElement _original;
		private readonly LayoutElement _final;
        private readonly LayoutElement _element;
		private readonly string _direction;

        public ResizeAction(LayoutElement element, string direction)
        {
            _element = element;
			_direction = direction;
			_original = element.Clone();
			_final = element.Clone();
        }

        public void Update(double dx, double dy)
        {
			if (_direction.Contains("Right"))
			{				
				_element.Width = Math.Max(_element.Width + dx, 1);
			}

			if (_direction.Contains("Bottom"))
			{
				_element.Height = Math.Max(_element.Height + dy, 1);
			}

			if (_direction.Contains("Left"))
			{
				double newWidth = Math.Max(_element.Width - dx, 1);

				_element.Left = _element.Left - (newWidth - _element.Width);
				_element.Width = newWidth;
			}

			if (_direction.Contains("Top"))
			{

				double newHeight = Math.Max(_element.Height - dy, 1);

				_element.Top = _element.Top - (newHeight - _element.Height);
				_element.Height = newHeight;
			}

			_final.CopyValuesFrom(_element);
        }

		public override void Undo()
		{
			_element.CopyValuesFrom(_original);
		}

		public override void Redo()
		{
			_element.CopyValuesFrom(_final);
		}
	}	
}

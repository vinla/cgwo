using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
	public class MoveAction : DesignerAction
    {
		private readonly List<LayoutElement> _elements;
		private Point _moveVector;

		public MoveAction(IEnumerable<LayoutElement> elements)
		{
			_elements = elements.ToList();
		}

		public void Update(Point moveVector)
		{
			_moveVector.Offset(moveVector.X, moveVector.Y);

			_elements.ForEach(el =>
			{
				el.Left += moveVector.X;
				el.Top += moveVector.Y;
			});
		}

		public override void Undo()
		{
			_elements.ForEach(el =>
			{
				el.Left -= _moveVector.X;
				el.Top -= _moveVector.Y;
			});
		}

		public override void Redo()
		{
			_elements.ForEach(el =>
			{
				el.Left += _moveVector.X;
				el.Top += _moveVector.Y;
			});
		}
	}
}

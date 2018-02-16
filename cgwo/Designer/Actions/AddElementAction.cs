using System.Collections.Generic;
using System.Linq;

namespace Cogs.Designer.Actions
{
	public class AddElementAction : DesignerAction
	{
		private readonly List<CardElement> _newElements;
		private readonly LayoutDocument _layoutDocument;

		public AddElementAction(LayoutDocument layoutDocument, CardElement newElement)
		{
			_layoutDocument = layoutDocument;
			_newElements = new List<CardElement> { newElement };
		}

		public AddElementAction(LayoutDocument layoutDocument, IEnumerable<CardElement> newElements)
		{
			_layoutDocument = layoutDocument;
			_newElements = newElements.ToList();
		}

		public override void Redo()
		{
			foreach(var element in _newElements)
				_layoutDocument.Elements.Add(element);
		}

		public override void Undo()
		{
			foreach(var element in _newElements)
				_layoutDocument.Elements.Remove(element);
		}
	}
}

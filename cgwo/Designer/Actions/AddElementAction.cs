using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogs.Designer.Actions
{
	public class AddElementAction : DesignerAction
	{
		private readonly CardElement _newElement;
		private readonly LayoutDocument _layoutDocument;

		public AddElementAction(LayoutDocument layoutDocument, CardElement newElement)
		{
			_layoutDocument = layoutDocument;
			_newElement = newElement;
		}

		public override void Redo()
		{
			_layoutDocument.Elements.Add(_newElement);
		}

		public override void Undo()
		{
			_layoutDocument.Elements.Remove(_newElement);
		}
	}
}

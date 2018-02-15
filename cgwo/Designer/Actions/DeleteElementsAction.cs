using System;
using System.Collections.Generic;
using System.Linq;

namespace Cogs.Designer.Actions
{
	public class DeleteElementsAction : DesignerAction
	{
		private readonly LayoutDocument _layoutDocument;
		private readonly List<CardElement> _snapshot;
		private readonly List<CardElement> _targets;
		public DeleteElementsAction(LayoutDocument layoutDocument, IEnumerable<CardElement> elementsToDelete)
		{
			_layoutDocument = layoutDocument;

			if (elementsToDelete.Any(el => _layoutDocument.Elements.IndexOf(el) < 0))
				throw new InvalidOperationException("element was not present in the layout document");

			_targets = elementsToDelete.ToList();
			_snapshot = new List<CardElement>();

			foreach (var element in layoutDocument.Elements)
			{
				if (elementsToDelete.Contains(element))
					_snapshot.Add(element.Clone() as CardElement);
				else
					_snapshot.Add(element);
			}
		}

		public override void Redo()
		{
			foreach (var element in _targets)
				_layoutDocument.Elements.Remove(element);
		}

		public override void Undo()
		{
			_layoutDocument.Elements.Clear();
			foreach (var element in _snapshot)
				_layoutDocument.Elements.Add(element);
		}
	}
}

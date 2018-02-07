using System;
using System.Collections.Generic;
using System.Linq;

namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
	public class DeleteElementAction : DesignerAction
	{
		private readonly LayoutDocument _layoutDocument;
		private List<LayoutElement> _snapshot;
		private List<LayoutElement> _targets;

		public DeleteElementAction(LayoutDocument layoutDocument, IEnumerable<LayoutElement> elements)
		{
			_layoutDocument = layoutDocument;

			if(elements.Any(el => layoutDocument.Elements.IndexOf(el) < 0))
				throw new InvalidOperationException("element was not present in the layout document");

			_targets = elements.ToList();
			_snapshot = new List<LayoutElement>();
			
			foreach (var element in layoutDocument.Elements)
			{
				if (elements.Contains(element))
					_snapshot.Add(element.Clone());
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

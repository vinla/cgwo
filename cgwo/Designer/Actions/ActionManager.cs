using System.Collections.Generic;
using System.Linq;

namespace Cogs.Designer.Actions
{
	public class ActionManager
	{
		private const int _maxActions = 15;
		private List<DesignerAction> _actions;
		private int _index;

		public ActionManager()
		{
			_actions = new List<DesignerAction>();
			_index = -1;
		}

		public void Push(DesignerAction action)
		{
			_index++;
			_actions = _actions.Take(_index).ToList();			
			_actions.Add(action);			
		}

		public void Undo()
		{
			if (_index > -1)
			{
				_actions[_index].Undo();
				_index--;
			}
		}

		public void Redo()
		{
			if (_index < _actions.Count - 1)
			{
				_index++;
				_actions[_index].Redo();
			}
		}

		public DesignerAction Latest => _index > -1 ? _actions[_index] : null;
	}
}

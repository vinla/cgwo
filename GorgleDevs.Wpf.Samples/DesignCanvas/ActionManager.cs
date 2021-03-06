﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorgleDevs.Wpf.Samples.DesignCanvas
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
	}
}

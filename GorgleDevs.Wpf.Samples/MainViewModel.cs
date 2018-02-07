using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GorgleDevs.Wpf.Mvvm;
using GorgleDevs.Wpf.Samples.DesignCanvas;

namespace GorgleDevs.Wpf.Samples
{
	public class MainViewModel : ViewModel
	{
		private readonly ActionManager _actionManager;
		private readonly LayoutDocument _layoutDocument;

		public MainViewModel()
		{
			_actionManager = new DesignCanvas.ActionManager();
			_layoutDocument = new LayoutDocument();
			_layoutDocument.Elements.Add(new LayoutElement
			{
				Top = 50,
				Left = 50,
				Height = 30,
				Width = 200
			});

			_layoutDocument.Elements.Add(new LayoutElement
			{
				Top = 150,
				Left = 150,
				Height = 30,
				Width = 30
			});

			_layoutDocument.Elements.Add(new LayoutElement
			{
				Top = 200,
				Left = 10,
				Height = 60,
				Width = 20
			});

		}		

		public LayoutDocument LayoutDocument => _layoutDocument;

		public DesignCanvas.ActionManager ActionManager => _actionManager;

		public ICommand Undo => new DelegateCommand(() =>
		{
			_actionManager.Undo();
		});

		public ICommand Redo => new DelegateCommand(() =>
		{
			_actionManager.Redo();
		});

		public ICommand Delete => new DelegateCommand(() =>
		{
			var deleteCommand = new DeleteElementAction(_layoutDocument, _layoutDocument.Elements.Where(el => el.Selected));
			deleteCommand.Redo();
			_actionManager.Push(deleteCommand);
		});

		public ICommand Add => new DelegateCommand(() =>
		{
			var element = new LayoutElement
			{
				Top = 10,
				Left = 10,
				Width = 100,
				Height = 20
			};
			var addCommand = new AddElementAction(_layoutDocument, element);
			addCommand.Redo();
			_actionManager.Push(addCommand);
		});
	}
}

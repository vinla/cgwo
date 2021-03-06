﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cogs.Designer;
using Cogs.Designer.Actions;
using GorgleDevs.Wpf;

namespace cgwo.Controls
{
	/// <summary>
	/// Interaction logic for DesignerCanvas.xaml
	/// </summary>
	public partial class DesignerCanvas : UserControl
	{
		private readonly Guidelines _guidelines;
		private readonly GuidelinesAdorner _guidelinesAdorner;
		private Rect _dragRect;
		private Point? _mouseDownPoint, _currentMousePoint, _lastKnownMousePoint;
		private CardElement _mouseDownTarget;
		private DesignerAction _currentAction;

		public DesignerCanvas()
		{
			InitializeComponent();
			_guidelines = new Guidelines();
			_guidelinesAdorner = new GuidelinesAdorner(this, _guidelines);
			Loaded += (s, e) =>
			{
				var adornerLayer = AdornerLayer.GetAdornerLayer(this);
				adornerLayer.Add(_guidelinesAdorner);
			};

			_guidelines.LinesUpdated += (s, e) =>
			{
				_guidelinesAdorner.InvalidateVisual();
			};
		}

		public static DependencyProperty ActionManagerProperty = DependencyProperty.Register(nameof(ActionManager), typeof(ActionManager), typeof(DesignerCanvas));

		public ActionManager ActionManager
		{
			get { return (ActionManager)GetValue(ActionManagerProperty); }
			set { SetValue(ActionManagerProperty, value); }
		}

		public static readonly DependencyProperty ElementsProperty = DependencyProperty.Register(nameof(Elements), typeof(IEnumerable<CardElement>), typeof(DesignerCanvas));

		public IEnumerable<CardElement> Elements
		{
			get { return (IEnumerable<CardElement>)GetValue(ElementsProperty); }
			set { SetValue(ElementsProperty, value); }
		}

		public static readonly DependencyProperty IsDisplayOnlyProperty = DependencyProperty.Register(nameof(IsDisplayOnly), typeof(bool), typeof(DesignerCanvas));

		public bool IsDisplayOnly
		{
			get { return (bool)GetValue(IsDisplayOnlyProperty); }
			set { SetValue(IsDisplayOnlyProperty, value); }
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left && !IsDisplayOnly)
			{
				_mouseDownPoint = e.GetPosition(this);
				_mouseDownTarget = Elements.Where(el => el.Bounds.Contains(_mouseDownPoint.Value)).LastOrDefault();

				if (_mouseDownTarget != null)
				{
					if (_mouseDownTarget.Selected == false && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) == false)
					{
						DeselectAll();
						_mouseDownTarget.Selected = true;
					}
					else if (_mouseDownTarget.Selected && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
						_mouseDownTarget.Selected = false;
					else
						_mouseDownTarget.Selected = true;
				}

				Mouse.Capture(this);
			}

			base.OnMouseDown(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (IsMouseDown)
			{
				_lastKnownMousePoint = _currentMousePoint;
				_currentMousePoint = e.GetPosition(this);
				UpdateDragRect();

				if (GetMouseAction() == MouseAction.DragItems)
					DragSelected();
			}

			base.OnMouseMove(e);
		}

		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left && !IsDisplayOnly)
			{
				var mouseAction = GetMouseAction();

				if (_currentAction is MoveAction)
				{
					_currentAction.SetComplete();
					ActionManager?.Push(_currentAction);
					_currentAction = null;
				}

				switch (mouseAction)
				{
					case MouseAction.Click:
						if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) == false)
						{
							foreach (var element in Elements.Where(el => el != _mouseDownTarget))
								element.Selected = false;
						}
						break;
					case MouseAction.DragSelect:
						SelectFromRect(_dragRect);
						break;
				}

				_mouseDownPoint = null;
				ReleaseMouseCapture();
				UpdateDragRect();
			}

			base.OnMouseUp(e);
		}

		private MouseAction GetMouseAction()
		{
			if (DragRectThresholdExceeded == false)
				return MouseAction.Click;
			else if (_mouseDownTarget == null)
				return MouseAction.DragSelect;
			return MouseAction.DragItems;
		}

		private bool IsMouseDown => _mouseDownPoint.HasValue;

		private void DeselectAll()
		{
			Elements.ToList().ForEach(el => el.Selected = false);
		}

		private void SelectFromRect(Rect selectionRect)
		{
			foreach (var element in Elements)
			{
				element.Selected = selectionRect.Contains(element.Bounds);
			}
		}

		private Rect GetDragRect()
		{
			if (_mouseDownPoint.HasValue == false)
				return new Rect(0, 0, 0, 0);

			int minX = (int)Math.Min(_mouseDownPoint.Value.X, _currentMousePoint.Value.X);
			int minY = (int)Math.Min(_mouseDownPoint.Value.Y, _currentMousePoint.Value.Y);
			int maxX = (int)Math.Max(_mouseDownPoint.Value.X, _currentMousePoint.Value.X);
			int maxY = (int)Math.Max(_mouseDownPoint.Value.Y, _currentMousePoint.Value.Y);

			return new Rect
			{
				X = minX,
				Y = minY,
				Width = maxX - minX,
				Height = maxY - minY
			};
		}

		private void UpdateDragRect()
		{
			_dragRect = GetDragRect();
			var action = GetMouseAction();

			if (action == MouseAction.DragSelect && DragRectThresholdExceeded)
			{
				SelectionRectangle.Visibility = Visibility.Visible;
				SelectionRectangle.SetValue(Canvas.LeftProperty, _dragRect.X);
				SelectionRectangle.SetValue(Canvas.TopProperty, _dragRect.Y);
				SelectionRectangle.SetValue(Canvas.WidthProperty, _dragRect.Width);
				SelectionRectangle.SetValue(Canvas.HeightProperty, _dragRect.Height);
			}
			else
				SelectionRectangle.Visibility = Visibility.Hidden;
		}

		private void DragSelected()
		{
			var moveAction = _currentAction as MoveAction
				?? new MoveAction(
					Elements.Where(el => el.Selected),
					_guidelines.GenerateGuidelines(Elements.Where(el => el.Selected == false).Select(el => el.Bounds)));

			moveAction.Update(MoveVector);
			_currentAction = moveAction;
		}

		private void ResizeCompleted(object sender, ResizeCompletedEventArgs e)
		{

			var resizeAction = _currentAction as ResizeAction;
			if (resizeAction != null)
			{
				resizeAction.SetComplete();
				ActionManager?.Push(resizeAction);
			}
		}

		private void ResizeStarted(object sender, ResizeStartedEventArgs e)
		{
			var overlay = sender as ResizeOverlay;
			var element = (overlay.DataContext as CardElement);
			_currentAction = new ResizeAction(element, e.Position.ToString(), _guidelines.GenerateGuidelines(Elements.Where(el => el != element).Select(el => el.Bounds)));
		}

		private void ResizeDelta(object sender, ResizeDeltaEventArgs e)
		{
			var resizeAction = _currentAction as ResizeAction;
			if (resizeAction != null && resizeAction.IsComplete == false)
				resizeAction.Update(e.Delta);
		}

		private bool DragRectThresholdExceeded => _dragRect.Size.Width > 3 || _dragRect.Size.Height > 3;

		private Vector MoveVector => new Vector((int)(_currentMousePoint.Value.X - _lastKnownMousePoint.Value.X), (int)(_currentMousePoint.Value.Y - _lastKnownMousePoint.Value.Y));
	}

	public enum MouseAction
	{
		None,
		Click,
		DragSelect,
		DragItems
	}
}

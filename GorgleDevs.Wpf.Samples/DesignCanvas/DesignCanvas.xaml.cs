using System;
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

namespace GorgleDevs.Wpf.Samples.DesignCanvas
{
    /// <summary>
    /// Interaction logic for DesignCanvas.xaml
    /// </summary>
    public partial class DesignCanvas : UserControl
    {
		private readonly Guidelines _guidelines;
		private Rect _dragRect;
        private Point? _mouseDownPoint, _currentMousePoint, _lastKnownMousePoint;
        private LayoutElement _mouseDownTarget;
        private Stack<DesignerAction> _actionStack;
		private DragAction _dragAction;
        
        public DesignCanvas()
        {
            InitializeComponent();
            _actionStack = new Stack<DesignerAction>();
			_guidelines = new Guidelines();
			_guidelines.SetGrid(10);
        }

        public static readonly DependencyProperty ElementsProperty = DependencyProperty.Register(nameof(Elements), typeof(IEnumerable<LayoutElement>), typeof(DesignCanvas));

        public IEnumerable<LayoutElement> Elements
        {
            get { return (IEnumerable<LayoutElement>)GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
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
            if (e.ChangedButton == MouseButton.Left)
            {
                var mouseAction = GetMouseAction();

                switch(mouseAction)
                {
                    case MouseAction.Click:
                        if(Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) == false)
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
			if (_dragAction == null)
				_dragAction = new DragAction(Elements.Where(el => el.Selected), _guidelines);
			_dragAction.Update(MoveVector);
        }
        private void ThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (_actionStack.Any())
            {
                var resizeAction = _actionStack.Peek() as ResizeAction;
                if (resizeAction != null)
                    resizeAction.SetComplete();
            }
        }

        private void ThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            var thumb = sender as Thumb;
            var element = (thumb.DataContext as LayoutElement);
            _actionStack.Push(new ResizeAction(element));
        }

        private void ThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_actionStack.Any())
            {
                var resizeAction = _actionStack.Peek() as ResizeAction;
                if (resizeAction != null && resizeAction.IsComplete == false)
                    resizeAction.Update(e.HorizontalChange, e.VerticalChange);
            }
        }

        private bool DragRectThresholdExceeded => _dragRect.Size.Width > 3 || _dragRect.Size.Height > 3;

        private Vector MoveVector => new Vector((int)(_currentMousePoint.Value.X - _lastKnownMousePoint.Value.X), (int)(_currentMousePoint.Value.Y - _lastKnownMousePoint.Value.Y));		
	}   
    
    public abstract class DesignerAction
    {
        public bool IsComplete { get; private set; }

        public void SetComplete()
        {
            IsComplete = true;
        }
    }

    public class MoveAction : DesignerAction
    {

    }

    public class ResizeAction : DesignerAction
    {        
        private readonly LayoutElement _element;        

        public ResizeAction(LayoutElement element)
        {
            _element = element;
        }

        public void Update(double dx, double dy)
        {                        
            _element.Width += dx;
            _element.Height += dy;
        }
    }

    public enum MouseAction
    {
        None,
        Click,
        DragSelect,
        DragItems
    }

	public class Guidelines
	{
		private int _gridSize;
		public void SetGrid(int gridSize)
		{
			_gridSize = gridSize;
		}

		public Point SnapPointToGrid(Point p)
		{
			int gridX = (int)(p.X / _gridSize) + 1;
			int gridY = (int)(p.Y / _gridSize) + 1;

			var distX = (gridX * _gridSize) - p.X;
			var distY = (gridY * _gridSize) - p.Y;

			p.X = distX < _gridSize / 2f ? gridX * _gridSize : (gridX - 1) * _gridSize;
			p.Y = distY < _gridSize / 2f ? gridY * _gridSize : (gridY - 1) * _gridSize;

			return p;
		}		
	}
}

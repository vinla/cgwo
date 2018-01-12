﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private Rect _dragRect;
        private Point? _mouseDownPoint, _currentMousePoint, _lastKnownMousePoint;
        private LayoutElement _mouseDownTarget;
        private Stack<DesignerAction> _actionStack;        
        
        public DesignCanvas()
        {
            InitializeComponent();
            _actionStack = new Stack<DesignerAction>();
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
            var moveVector = MoveVector;
            foreach (var element in Elements.Where(el => el.Selected))
            {
                element.Top += moveVector.Y;
                element.Left += moveVector.X;
            }
        }

        private void ThumbMouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private bool DragRectThresholdExceeded => _dragRect.Size.Width > 3 || _dragRect.Size.Height > 3;

        private Point MoveVector => new Point((int)(_currentMousePoint.Value.X - _lastKnownMousePoint.Value.X), (int)(_currentMousePoint.Value.Y - _lastKnownMousePoint.Value.Y));
    }   
    
    public abstract class DesignerAction
    {

    }

    public class MoveAction : DesignerAction
    {

    }

    public class ResizeAction : DesignerAction
    {

    }

    public enum MouseAction
    {
        None,
        Click,
        DragSelect,
        DragItems
    }
}
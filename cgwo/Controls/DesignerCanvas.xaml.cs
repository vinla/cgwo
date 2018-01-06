using System;
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
using Cogs.Designer;

namespace cgwo.Controls
{
    /// <summary>
    /// Interaction logic for DesignerCanvas.xaml
    /// </summary>
    public partial class DesignerCanvas : UserControl
    {
        private HighlightAdorner _adorner;
        private ResizeAdorner _resizeAdorner;
        private FrameworkElement _selection;

        public DesignerCanvas()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ElementsProperty = DependencyProperty.Register(nameof(Elements), typeof(IEnumerable<CardElement>), typeof(DesignerCanvas));

        public IEnumerable<CardElement> Elements
        {
            get { return (IEnumerable<CardElement>)GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }

        public static readonly DependencyProperty SelectedElementProperty = DependencyProperty.Register(nameof(SelectedElement), typeof(CardElement), typeof(DesignerCanvas));

        public CardElement SelectedElement
        {
            get { return (CardElement)GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); UpdateSelection(); }
        }

        public static readonly DependencyProperty IsDisplayOnlyProperty = DependencyProperty.Register(nameof(IsDisplayOnly), typeof(bool), typeof(DesignerCanvas));

        public bool IsDisplayOnly
        {
            get { return (bool)GetValue(IsDisplayOnlyProperty); }
            set { SetValue(IsDisplayOnlyProperty, value); }
        }

        private void ItemMouseEnter(object sender, MouseEventArgs e)
        {
            if (IsDisplayOnly)
                return;

            var item = sender as FrameworkElement;
            _adorner = new HighlightAdorner(item);
            AdornerLayer.GetAdornerLayer(item).Add(_adorner);
        }

        private void ItemMouseLeave(object sender, MouseEventArgs e)
        {
            var item = sender as FrameworkElement;
            AdornerLayer.GetAdornerLayer(item)?.Remove(_adorner);
        }

        private void UpdateSelection()
        {
			Focus();
            Deselect();

            for (int i = 0; i < itemsHost.Items.Count; i++)
            {
                var uiElement = (FrameworkElement)itemsHost.ItemContainerGenerator.ContainerFromIndex(i);
                if (uiElement.DataContext == SelectedElement)
                {
                    _resizeAdorner = new ResizeAdorner(uiElement);
                    AdornerLayer.GetAdornerLayer(uiElement).Add(_resizeAdorner);
                    _selection = uiElement;
                }
            }
        }

        private void Deselect()
        {
            if (_selection != null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(_selection);
                if(adornerLayer != null)
                    adornerLayer.Remove(_resizeAdorner);
            }
        }

        private void ItemMouseClick(object sender, MouseButtonEventArgs e)
        {
            if (IsDisplayOnly)
                return;

            var frameworkElement = (sender as FrameworkElement);
            if (frameworkElement != null && _selection != frameworkElement)
                SelectedElement = frameworkElement.DataContext as CardElement;
        }

        private void CanvasClick(object sender, MouseButtonEventArgs e)
        {
            if (IsDisplayOnly)
                return;

            if (sender == e.OriginalSource)
                Deselect();
        }

		protected override void OnKeyDown(KeyEventArgs e)
		{						
			if (SelectedElement == null)
				return;
			var moveAmount = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift) ? 5 : 1;

			if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
			{
				if (e.Key == Key.Right)
					SelectedElement.Width += moveAmount;
				else if (e.Key == Key.Left)
					SelectedElement.Width = Math.Max(1, SelectedElement.Width - moveAmount);
				else if (e.Key == Key.Up)
					SelectedElement.Height = Math.Max(1, SelectedElement.Height - moveAmount);
				else if (e.Key == Key.Down)
					SelectedElement.Height += moveAmount;
			}
			else
			{				
				if (e.Key == Key.Right)
					SelectedElement.Left += moveAmount;
				else if (e.Key == Key.Left)
					SelectedElement.Left -= moveAmount;
				else if (e.Key == Key.Up)
					SelectedElement.Top -= moveAmount;
				else if (e.Key == Key.Down)
					SelectedElement.Top += moveAmount;
			}

			e.Handled = true;
		}
	}
}

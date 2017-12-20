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

        public DependencyProperty ElementsProperty = DependencyProperty.Register(nameof(Elements), typeof(IEnumerable<CardElement>), typeof(DesignerCanvas));

        public IEnumerable<CardElement> Elements
        {
            get { return (IEnumerable<CardElement>)GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }

        public DependencyProperty SelectedElementProperty = DependencyProperty.Register(nameof(SelectedElement), typeof(CardElement), typeof(DesignerCanvas));

        public CardElement SelectedElement
        {
            get { return (CardElement)GetValue(SelectedElementProperty); }
            set { SetValue(SelectedElementProperty, value); UpdateSelection(); }
        }

        private void ItemMouseEnter(object sender, MouseEventArgs e)
        {
            var item = sender as FrameworkElement;
            _adorner = new HighlightAdorner(item);
            AdornerLayer.GetAdornerLayer(item).Add(_adorner);
        }

        private void ItemMouseLeave(object sender, MouseEventArgs e)
        {
            var item = sender as FrameworkElement;
            AdornerLayer.GetAdornerLayer(item).Remove(_adorner);
        }

        private void UpdateSelection()
        {
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
            var frameworkElement = (sender as FrameworkElement);
            if (frameworkElement != null && _selection != frameworkElement)
                SelectedElement = frameworkElement.DataContext as CardElement;
        }

        private void CanvasClick(object sender, MouseButtonEventArgs e)
        {
            if (sender == e.OriginalSource)
                Deselect();
        }
    }
}

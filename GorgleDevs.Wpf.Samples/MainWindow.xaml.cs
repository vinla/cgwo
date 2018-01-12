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

namespace GorgleDevs.Wpf.Samples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new
            {
                Elements = new[]
                {
                    new DesignCanvas.LayoutElement{ Left = 100, Top = 100, Width = 100, Height = 25},
                    new DesignCanvas.LayoutElement{ Left = 100, Top = 200, Width = 25, Height = 25}
                }
            };
        }
    }
}

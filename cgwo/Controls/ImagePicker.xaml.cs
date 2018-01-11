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

namespace cgwo.Controls
{
    /// <summary>
    /// Interaction logic for ImagePicker.xaml
    /// </summary>
    public partial class ImagePicker : UserControl
    {
        public ImagePicker()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof(ImageData), typeof(IList<byte>), typeof(ImagePicker), new PropertyMetadata(ImageSourceChangedCallback));

        public IList<byte> ImageData
        {
            get { return (IList<byte>)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        private void ChooseImageClick(object sender, RoutedEventArgs e)
        {
            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.Filter = "Image files|*.png;*.jpg;*.bmp;*.tga;*.gif";
                if(openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ImageData = System.IO.File.ReadAllBytes(openFileDialog.FileName);
                }
            }
        }

        private void UpdatePreview(IList<byte> data)
        {
            if (data != null && data.Count > 0)
                Preview.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(data.ToArray());
            else
                Preview.Source = null;
        }

        private static void ImageSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ImagePicker imagePicker)
            {
                imagePicker.UpdatePreview((IList<byte>)e.NewValue);
            }
        }
    }
}

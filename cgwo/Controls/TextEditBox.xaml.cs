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
	/// Interaction logic for TextEditBox.xaml
	/// </summary>
	public partial class TextEditBox : UserControl
	{
		public TextEditBox()
		{
			InitializeComponent();
		}		

		public static readonly DependencyProperty IsEditingProperty = DependencyProperty.Register(nameof(IsEditing), typeof(bool), typeof(TextEditBox));

		public bool IsEditing
		{
			get { return (bool)GetValue(IsEditingProperty); }
			set { SetValue(IsEditingProperty, value); }
		}

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(TextEditBox));

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		private void TextBlockClick(object sender, MouseButtonEventArgs e)
		{			
			if (e.ClickCount == 2)
			{
				IsEditing = true;
				Editor.Focus();
			}
		}

		private void TextBoxLostFocus(object sender, RoutedEventArgs e)
		{
			IsEditing = false;
		}		
	}
}

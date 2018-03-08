using System;
using System.Globalization;
using System.Windows.Data;

namespace cgwo.Wpf
{
	public class ImageSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null)
				return Project.DataStore.Images.Retrieve(value.ToString());
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}

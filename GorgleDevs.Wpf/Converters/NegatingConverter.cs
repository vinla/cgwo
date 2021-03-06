﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace GorgleDevs.Wpf.Converters
{
	public class NegatingConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool test)
				return !test;

			throw new InvalidOperationException("Value must be a boolean");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Convert(value, targetType, parameter, culture);
		}
	}
}

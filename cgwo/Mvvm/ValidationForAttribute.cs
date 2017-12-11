using System;

namespace cgwo.Mvvm
{
    public class ValidationForAttribute : Attribute
    {
		private readonly string _propertyName;

		public ValidationForAttribute(string propertyName)
		{
			_propertyName = propertyName;
		}

		public string PropertyName => _propertyName;
    }
}

using System;

namespace GorgleDevs.Wpf.Mvvm
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CalculateFromAttribute : Attribute
    {
        private readonly string _propertyName;

        public CalculateFromAttribute(string propertyName)
        {
            _propertyName = propertyName;
        }

        public string PropertyName => _propertyName;
    }
}

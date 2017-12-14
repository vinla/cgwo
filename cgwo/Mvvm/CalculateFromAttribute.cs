using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cgwo.Mvvm
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

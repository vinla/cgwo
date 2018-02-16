using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogs.Designer.Actions
{
	public class ChangePropertyAction : DesignerAction
	{
		private readonly CardElement _element;
		private readonly string _propertyName;
		private readonly object _initialValue;
		private object _finalValue;

		public ChangePropertyAction(CardElement element, string propertyName, object initialValue)
		{
			_element = element;
			_propertyName = propertyName;
			_initialValue = initialValue ?? GetPropertyValue();
		}

		public CardElement Element => _element;

		public string PropertyName => _propertyName;

		public void Update(object value)
		{
			//SetPropertyValue(value);
			_finalValue = value;
		}

		public override void Redo()
		{
			SetPropertyValue(_finalValue);
		}

		public override void Undo()
		{
			SetPropertyValue(_initialValue);
		}

		private object GetPropertyValue()
		{
			return _element.GetType().GetProperty(_propertyName).GetValue(_element);
		}

		private void SetPropertyValue(object value)
		{
			_element.GetType().GetProperty(_propertyName).SetValue(_element, value);
		}		
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace cgwo.Mvvm
{
	public abstract class ViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
	{
		private readonly Dictionary<string, object> _values;
		private readonly Dictionary<string, List<MethodInfo>> _validators;
		private readonly Dictionary<string, List<string>> _errors;

		public bool HasErrors => _errors.Select(e => e.Value.Any()).Any(b => b);

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		protected ViewModel()
		{
			_values = new Dictionary<string, object>();
			_validators = new Dictionary<string, List<MethodInfo>>();
			_errors = new Dictionary<string, List<string>>();
		}

		public void RaisePropertyChanged(string propertyName)
		{			
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void SetValue(string name, object value)
		{
			if (_values.ContainsKey(name))
				_values.Remove(name);
			_values.Add(name, value);
			ValidateProperty(name, value);
			RaisePropertyChanged(name);
			RaisePropertyChanged(nameof(HasErrors));
		}

		protected T GetValue<T>(string name)
		{
			if (_values.ContainsKey(name) == false)
				return default(T);
			return (T)_values[name];
		}

		public IEnumerable GetErrors(string propertyName)
		{
			if (_errors.ContainsKey(propertyName))
				return _errors[propertyName];
			else
				return null;
		}

		private void ValidateProperty(string propertyName, object value)
		{
			if (_validators.ContainsKey(propertyName) == false)
				_validators.Add(propertyName, GetValidationMethods(propertyName).ToList());

			_errors.Remove(propertyName);

			var errors =
				_validators[propertyName]
					.Select(v => v.Invoke(this, new[] { value }) as string)
					.Where(s => s != null)
					.ToList();

			_errors.Add(propertyName, errors);
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}

		private IEnumerable<MethodInfo> GetValidationMethods(string propertyName)
		{
			return
				GetType()
				.GetMethods(BindingFlags.Public| BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
				.Where(m => m.GetCustomAttributes<ValidationForAttribute>().Any(attr => attr.PropertyName == propertyName));
		}
	}
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Cogs.Designer.Actions;
using Xceed.Wpf.Toolkit;

namespace Cogs.Designer
{
	public class ValueChangeTrackerBehaviour : Behavior<FrameworkElement>
	{
		private ValueChangeMonitor _changeMonitor;

		public static readonly DependencyProperty ActionManagerProperty = DependencyProperty.Register(nameof(ActionManager), typeof(ActionManager), typeof(ValueChangeTrackerBehaviour));

		public ActionManager ActionManager
		{
			get { return (ActionManager)GetValue(ActionManagerProperty); }
			set { SetValue(ActionManagerProperty, value); }
		}

		public static readonly DependencyProperty CardElementProperty = DependencyProperty.Register(nameof(CardElement), typeof(CardElement), typeof(ValueChangeTrackerBehaviour));

		public CardElement CardElement
		{
			get { return (CardElement)GetValue(CardElementProperty); }
			set { SetValue(CardElementProperty, value); }
		}

		public String PropertyName
		{
			get; set;
		}

		protected override void OnAttached()
		{
			AssociatedObject.LostFocus += AssociatedObject_LostFocus;
			_changeMonitor = CreateValueMonitor(AssociatedObject);
			_changeMonitor.OnAttached();
			base.OnAttached();
		}		

		protected override void OnDetaching()
		{
			AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
			if (_changeMonitor != null)
				_changeMonitor.OnDetaching();
			base.OnDetaching();
		}

		protected void UpdateProperty(object oldValue, object newValue)
		{
			if (AssociatedObject.IsKeyboardFocusWithin == false)
				return;

			if (ActionManager != null)
			{
				var changePropertyAction = ActionManager.Latest as ChangePropertyAction;
				if (changePropertyAction == null || changePropertyAction.Element != CardElement || changePropertyAction.PropertyName != PropertyName || changePropertyAction.IsComplete)
				{
					changePropertyAction = new ChangePropertyAction(CardElement, PropertyName, oldValue);
					ActionManager.Push(changePropertyAction);
				}

				changePropertyAction.Update(newValue);
			}
		}

		private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
		{
			if (ActionManager != null)
			{
				var changePropertyAction = ActionManager.Latest as ChangePropertyAction;
				if (changePropertyAction != null && changePropertyAction.Element == CardElement && changePropertyAction.PropertyName == PropertyName)
				{
					changePropertyAction.SetComplete();
				}
			}
		}

		private ValueChangeMonitor CreateValueMonitor(DependencyObject associatedObject)
		{
			if (associatedObject is DoubleUpDown doubleUpDown)
				return new DoubleValueChangeMonitor(doubleUpDown, UpdateProperty);
			if (associatedObject is SingleUpDown singleUpDown)
				return new SingleValueChangeMonitor(singleUpDown, UpdateProperty);
			if (associatedObject is TextBox textBox)
				return new TextBoxChangeMonitor(textBox, UpdateProperty);

			throw new InvalidOperationException("Unexpected input type");
		}
	}

	public abstract class ValueChangeMonitor
	{
		private readonly Action<object, object> _onValueChanged;

		protected ValueChangeMonitor(Action<object, object> onValueChanged)
		{
			_onValueChanged = onValueChanged ?? throw new ArgumentNullException(nameof(onValueChanged));
		}

		public abstract void OnAttached();
		public abstract void OnDetaching();

		protected Action<object, object> OnValueChanged => _onValueChanged;
	}

	public abstract class ValueChangeMonitor<T> : ValueChangeMonitor where T : DependencyObject
	{
		private readonly T _associatedObject;

		public ValueChangeMonitor(T associatedObject, Action<object, object> onValueChanged) : base(onValueChanged)
		{
			_associatedObject = associatedObject ?? throw new ArgumentNullException();
		}

		public T AssociatedObject => _associatedObject;
	}

	public class SingleValueChangeMonitor : ValueChangeMonitor<SingleUpDown>
	{
		public SingleValueChangeMonitor(SingleUpDown associatedObject, Action<object, object> onValueChanged) : base(associatedObject, onValueChanged)
		{ }

		public override void OnAttached()
		{
			AssociatedObject.ValueChanged += AssociatedObject_ValueChanged;			
		}

		public override void OnDetaching()
		{
			AssociatedObject.ValueChanged -= AssociatedObject_ValueChanged;			
		}

		private void AssociatedObject_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			OnValueChanged(e.OldValue, e.NewValue);
		}
	}

	public class DoubleValueChangeMonitor : ValueChangeMonitor<DoubleUpDown>
	{
		public DoubleValueChangeMonitor(DoubleUpDown associatedObject, Action<object, object> onValueChanged) : base(associatedObject, onValueChanged)
		{ }

		public override void OnAttached()
		{
			AssociatedObject.ValueChanged += AssociatedObject_ValueChanged;
		}

		public override void OnDetaching()
		{
			AssociatedObject.ValueChanged -= AssociatedObject_ValueChanged;
		}

		private void AssociatedObject_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			OnValueChanged(e.OldValue, e.NewValue);
		}
	}

	public class TextBoxChangeMonitor: ValueChangeMonitor<TextBox>
	{
		string _previousValue;
		public TextBoxChangeMonitor(TextBox associatedObject, Action<object, object> onValueChanged) : base(associatedObject, onValueChanged)
		{ }

		public override void OnAttached()
		{
			AssociatedObject.TextChanged += AssociatedObject_TextChanged;
			_previousValue = AssociatedObject.Text;
		}

		public override void OnDetaching()
		{
			AssociatedObject.TextChanged -= AssociatedObject_TextChanged;			
		}

		private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
		{
			OnValueChanged(_previousValue, AssociatedObject.Text);
			_previousValue = AssociatedObject.Text;
		}
	}
}

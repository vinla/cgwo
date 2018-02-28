using Cogs.Common;
using GorgleDevs.Wpf.Mvvm;

namespace Cogs.Designer
{
	public abstract class NamedValueViewModel : ViewModel
	{
		public abstract string Name { get; }

		public abstract string Value { get; set; }

		public virtual string Editor => string.Empty;

		public string ValueOrDefault => string.IsNullOrEmpty(Value) ? "{"+Name+"}" : Value;
	}

	public class CardNameValueViewModel : NamedValueViewModel
	{
		private readonly Card _card;

		public CardNameValueViewModel(Card card)
		{
			_card = card;
		}

		public override string Name => "Name";

		public override string Value
		{
			get => _card.Name;
			set 
			{
				_card.Name = value;
				RaisePropertyChanged(nameof(Value));
			}
		}

		public override string Editor => "TextEditor";
	}

	public class CardTypeValueViewModel : NamedValueViewModel
	{
		private readonly Card _card;

		public CardTypeValueViewModel(Card card)
		{
			_card = card;
		}

		public override string Name => "Type";

		public override string Value { get => _card.CardType.Name; set => throw new System.NotSupportedException(); }
	}

	public class CardAttributeValueViewModel : NamedValueViewModel
	{
		private readonly CardAttributeValue _cardAttributeValue;

		public CardAttributeValueViewModel(CardAttributeValue cardAttributeValue)
		{
			_cardAttributeValue = cardAttributeValue;
		}

		public override string Name => _cardAttributeValue.CardAttribute.Name;

		public override string Value
		{
			get { return _cardAttributeValue.Value; }
			set
			{
				_cardAttributeValue.Value = value;
				RaisePropertyChanged(nameof(Value));
			}
		}

		public override string Editor
		{
			get
			{
				switch(_cardAttributeValue.CardAttribute.Type)
				{
					case AttributeType.Text:
						return "TextEditor";
					case AttributeType.Image:
						return "ImageEditor";
					default:
						throw new System.NotSupportedException();
				}
			}
		}
	}
}

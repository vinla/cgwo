using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cogs.Common;

namespace cgwo.ViewModels
{
	public class ProjectViewModel : Mvvm.ViewModel
	{
        private ICardGameDataStore _cardGameDataStore;
        private readonly string _projectName;
		private Data.CardTypeViewModel _cardType;
		
		public ProjectViewModel(ICardGameDataStore cardGameDataStore)
		{
            _cardGameDataStore = cardGameDataStore ?? throw new ArgumentNullException(nameof(cardGameDataStore));
            _projectName = _cardGameDataStore.GetProjectInfo().Name;
		}

		public string Title => $"Project > {_projectName}";

		public IEnumerable<object> CardTypes => _cardGameDataStore.GetCardTypes().Select(x => x);

		public Data.CardTypeViewModel CardType => _cardType;

		public CardType SelectedCardType
		{
			get { return GetValue<CardType>(nameof(SelectedCardType)); }
			set
			{
				if (_cardType != null && value != null)
				{
					if (_cardType.Id == value.Id)
						return;

					if (_cardType.HasChanges)
					{
						if (System.Windows.Forms.MessageBox
								.Show("Do you want to swap and lose unsaved changes", "Unsaved changes", 
									System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
							return;
					}
				}

				SetValue(nameof(SelectedCardType), value);

				if (value != null)
				{
					_cardType = new Data.CardTypeViewModel(_cardGameDataStore, SelectedCardType);
					RaisePropertyChanged(nameof(CardType));
				}
			}
		}

		public ICommand AddType => new Mvvm.DelegateCommand(() =>
		{
			var newType = new CardType
			{
				Name = String.Empty
			};

			_cardType = new Data.CardTypeViewModel(_cardGameDataStore, newType);
			
			RaisePropertyChanged(nameof(CardType));
		});

		public ICommand Save => new Mvvm.DelegateCommand(() =>
		{
			_cardType.Save();
			RaisePropertyChanged(nameof(CardTypes));
		});
	}
}

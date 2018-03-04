using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cogs.Common;
using GorgleDevs.Wpf.Mvvm;

namespace cgwo.ViewModels.Data
{
    public class CardAttributeViewModel : ViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly ICardGameDataStore _cardGameDataStore;
        private IEnumerable<CardAttribute> _existingAttributes;
        private readonly Guid _cardTypeId;
        private readonly CardAttribute _original;
        private readonly Action _cancelEdit;
        private bool _isDeleted;

        public CardAttributeViewModel(ICardGameDataStore cardGameDataStore, IDialogService dialogService, Guid cardTypeId, CardAttribute original, Action cancelEdit, Action updated)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _cardGameDataStore = cardGameDataStore ?? throw new ArgumentNullException(nameof(cardGameDataStore));
            _original = original ?? throw new ArgumentNullException(nameof(original));
            _cardTypeId = cardTypeId;
            _cancelEdit = cancelEdit;
            _existingAttributes = _cardGameDataStore.GetCardAttributes(_cardTypeId);
            _isDeleted = false;
            Name = _original.Name ?? String.Empty;

            PropertyChanged += (s, e) =>
            {
                if(e.PropertyName == nameof(HasChanges))
                {
                    updated?.Invoke();
                }
            };
        }

        public string Name
        {
            get { return GetValue<string>(nameof(Name)); }
            set { SetValue(nameof(Name), value); }
        }

        public AttributeType Type => _original.Type;

        [CalculateFrom(nameof(Name))]
        public bool HasChanges => IsNew || Name != _original.Name;

        public bool IsNew => _existingAttributes.Any(ca => ca.Id == _original.Id) == false;

        [CalculateFrom(nameof(Name))]
        public bool AttributeNameInUse => _existingAttributes.Any(ca => ca.Name == Name.Trim() && ca.Id != _original.Id);

        [CalculateFrom(nameof(Name))]
        public bool IsValid => String.IsNullOrEmpty(Name) == false && IsPropertyValid(nameof(Name)) && !AttributeNameInUse;

        public bool IsDeleted => _isDeleted;

        public ICommand CancelCommand => new DelegateCommand(() =>
        {
            Name = _original.Name ?? String.Empty;
            _cancelEdit?.Invoke();
        });

        public ICommand SaveCommand => new DelegateCommand(() =>
        {
            if (!IsValid)
                return;

            Name = Name.Trim();
            _original.Name = Name;
            _cardGameDataStore.SaveCardAttribute(_cardTypeId, _original);
			_existingAttributes = _cardGameDataStore.GetCardAttributes(_cardTypeId);

            RaisePropertyChanged(nameof(IsNew));
            _cancelEdit?.Invoke();
        });

        public ICommand DeleteCommand => new DelegateCommand(() =>
        {
            if (IsNew)
                return;

            if (_dialogService.Prompt("Confirm delete", "Are you sure you want to delete this attribute?") == DialogResult.Accept)
            {
                _cardGameDataStore.DeleteCardAttribute(_original);
				_existingAttributes = _cardGameDataStore.GetCardAttributes(_cardTypeId);
				_isDeleted = true;
                _cancelEdit?.Invoke();
            }
        });

        [ValidationFor(nameof(Name))]
        private string ValidateProjectName(object value)
        {
            var whitelistRegex = "^[a-zA-Z0-9]*$";
            if (System.Text.RegularExpressions.Regex.IsMatch(Name, whitelistRegex) == false)
                return "Attribute names can only contain numbers letter or one of the following special characters _-&+";
            return null;
        }
    }
}

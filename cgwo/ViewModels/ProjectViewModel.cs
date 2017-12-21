using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using cgwo.Mvvm;
using Cogs.Common;

namespace cgwo.ViewModels
{
    public class ProjectViewModel : ViewModel
    {
        private readonly ICardGameDataStore _cardGameDataStore;
        private readonly IDialogService _dialogService;
        private string _currentModuleName;

        public ProjectViewModel(ICardGameDataStore cardGameDataStore, IDialogService dialogService)
        {
            _cardGameDataStore = cardGameDataStore ?? throw new ArgumentNullException();
            _dialogService = dialogService ?? throw new ArgumentNullException();
            LoadModule(ModuleNames.CardTypes);
        }

        public ICommand LoadModuleCommand => new DelegateCommand(o =>
        {
            var moduleToLoad = o.ToString();
            LoadModule(moduleToLoad);            
        });

        public ModuleViewModel CurrentModule
        {
            get { return GetValue<ModuleViewModel>(nameof(CurrentModule)); }
            private set { SetValue(nameof(CurrentModule), value); }
        }

        [Mvvm.CalculateFrom(nameof(CurrentModule))]
        public string CurrentModuleName => _currentModuleName;

        private void LoadModule(string moduleToLoad)
        {
            if (_currentModuleName == moduleToLoad)
                return;

            if (CurrentModule == null || CurrentModule.BeforeUnload())
            {
                switch (moduleToLoad)
                {
                    case ModuleNames.CardTypes:
                        _currentModuleName = ModuleNames.CardTypes;
                        CurrentModule = new CardTypesViewModel(_cardGameDataStore, _dialogService);                        
                        break;
                    case ModuleNames.Cards:
                        _currentModuleName = ModuleNames.Cards;
                        CurrentModule = new CardsViewModel();
                        break;
                }
            }
        }
    }

    public static class ModuleNames
    {
        public const string CardTypes = "Card Types";
        public const string Cards = "Cards";
        public const string Rules = "Rules";
    }
}

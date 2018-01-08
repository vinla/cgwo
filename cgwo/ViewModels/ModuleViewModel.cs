using GorgleDevs.Wpf.Mvvm;

namespace cgwo.ViewModels
{
    public abstract class ModuleViewModel : ViewModel
    {
        public abstract bool BeforeUnload();
    }
}

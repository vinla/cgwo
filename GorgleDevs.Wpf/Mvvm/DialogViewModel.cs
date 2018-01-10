namespace GorgleDevs.Wpf.Mvvm
{
    public class DialogViewModel : ViewModel
    {
        private DialogResult _result = DialogResult.None;
        
        protected void SetResult(DialogResult result)
        {
            _result = result;
            RaisePropertyChanged(nameof(Result));
        }

        public DialogResult Result => _result;
    }
}

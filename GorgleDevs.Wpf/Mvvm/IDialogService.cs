namespace GorgleDevs.Wpf.Mvvm
{
    public interface IDialogService
    {
        void Message(string message);
        DialogResult Prompt(string subject, string message);
        (DialogResult Result, string Path) ChooseFolder(string startingPath);
        (DialogResult Result, string Path) ChooseFile(string startingPath, string fileFilter);
    }

    public enum DialogResult
    {
        Accept,
        Reject
    }
}

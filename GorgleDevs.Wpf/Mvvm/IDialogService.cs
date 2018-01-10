using System.Threading.Tasks;

namespace GorgleDevs.Wpf.Mvvm
{
    public interface IDialogService
    {
        void Message(string message);
        DialogResult Prompt(string subject, string message);
        (DialogResult Result, string Path) ChooseFolder(string startingPath);
        (DialogResult Result, string Path) ChooseFile(string startingPath, string fileFilter);
        Task<DialogResult> ShowDialog(DialogViewModel viewModel);
    }

    public enum DialogResult
    {
        None,
        Accept,
        Reject
    }
}

using System;

namespace cgwo.Mvvm
{
    public class WindowsDialogService : IDialogService
    {
        public (DialogResult Result, string Path) ChooseFile(string startingPath, string fileFilter)
        {
            using (var dlg = new System.Windows.Forms.OpenFileDialog())
            {
                dlg.InitialDirectory = startingPath;
                dlg.Filter = fileFilter;
                if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return (DialogResult.Accept, dlg.FileName);
                }
            }

            return (DialogResult.Reject, String.Empty);
        }

        public (DialogResult Result, string Path) ChooseFolder(string startingPath)
        {
            using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return (DialogResult.Accept, dlg.SelectedPath);
                }
            }

            return (DialogResult.Reject, String.Empty);
        }

        public DialogResult Prompt(string subject, string message)
        {
            var result = System.Windows.Forms.MessageBox.Show(message, subject, System.Windows.Forms.MessageBoxButtons.YesNo);
            switch(result)
            {
                case System.Windows.Forms.DialogResult.Yes:
                    return DialogResult.Accept;
                case System.Windows.Forms.DialogResult.No:
                    return DialogResult.Reject;
                default:
                    throw new InvalidOperationException("unexpected dialog result");
            }
        }
    }
}

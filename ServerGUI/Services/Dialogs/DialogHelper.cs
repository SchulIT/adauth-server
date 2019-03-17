using System;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ServerGUI.Services.Dialogs
{
    public class DialogHelper : IDialogHelper
    {
        public string BrowseFile()
        {
            var dialog = new CommonOpenFileDialog();

            dialog.IsFolderPicker = false;
            dialog.Multiselect = false;
            dialog.EnsureFileExists = true;
            dialog.EnsureFileExists = true;
            dialog.Filters.Add(new CommonFileDialogFilter("Zertifikat", "*.pfx"));

            var result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }

            return null;
        }

        public void ShowException(Exception e)
        {
            var dialog = new TaskDialog();
            dialog.Opened += delegate
            {
                dialog.Icon = TaskDialogStandardIcon.Error;
            };

            dialog.Icon = TaskDialogStandardIcon.Error;

            dialog.Caption = e.GetType().ToString();
            dialog.Text = e.Message;

            dialog.Show();
        }
    }
}

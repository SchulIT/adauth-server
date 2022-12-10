using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Application = System.Windows.Application;

namespace AuthServer.GUI.UI
{
    public class DialogHelper : IDialogHelper
    {
        public string BrowseFile()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                Filter = "Certificate (*.pfx)|*.pfx"
            };

            var result = dialog.ShowDialog();

            if (result == true)
            {
                return dialog.FileName;
            }

            return null;
        }

        public void ShowException(Exception e)
        {
            var page = new TaskDialogPage
            {
                Caption = "Fehler",
                Text = "Ein Fehler ist aufgetreten",
                Icon = TaskDialogIcon.Error,
                Expander =
                {
                    Text = e.Message,
                    Expanded = true
                }
            };

            var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);


            if (activeWindow != null)
            {
                TaskDialog.ShowDialog(new WindowInteropHelper(activeWindow).Handle, page);
            }
            else
            {
                TaskDialog.ShowDialog(page);
            }
        }
    }
}

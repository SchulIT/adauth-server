using System;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using KPreisser.UI;
using Microsoft.Win32;

namespace AuthServer.GUI.UI
{
    public class DialogHelper : IDialogHelper
    {
        public string BrowseFile()
        {
            var dialog = new OpenFileDialog();

            dialog.CheckFileExists = true;
            dialog.Multiselect = false;
            dialog.Filter = "Certificate (*.pfx)|*.pfx";

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
                Title = Strings.Strings.ErrorTitle,
                Text = Strings.Strings.ErrorText,
                Instruction = e.GetType().ToString(),
                Expander =
                {
                    Text = e.Message,
                    ExpandFooterArea = true
                }
            };

            var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

            var dialog = new TaskDialog(page);

            if (activeWindow != null)
            {
                dialog.Show(new WindowInteropHelper(activeWindow).Handle);
            }
            else
            {
                dialog.Show();
            }
        }
    }
}

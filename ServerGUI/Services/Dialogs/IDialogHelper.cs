using System;

namespace ServerGUI.Services.Dialogs
{
    public interface IDialogHelper
    {
        string BrowseFile();

        void ShowException(Exception e);
    }
}

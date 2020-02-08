using System;

namespace AuthServer.GUI.UI
{
    public interface IDialogHelper
    {
        string BrowseFile();

        void ShowException(Exception e);
    }
}

using System;

namespace AuthServer.GUI.UI
{
    public interface IDispatcher
    {
        void RunOnUI(Action action);
    }
}

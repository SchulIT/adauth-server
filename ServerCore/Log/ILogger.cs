using System;

namespace ServerCore.Log
{ 
    /// <summary>
    /// Interface for logger
    /// </summary>
    public interface ILogger
    {
        void Initialize();

        void WriteException(Exception e);

        void WriteDebug(string message);

        void WriteLog(string message);
    }
}

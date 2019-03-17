using System;

namespace ServerCore.Log
{
    /// <summary>
    /// Logs messages to the console.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        public void Initialize() { }

        public void WriteException(Exception e)
        {
            var message = $"Type: {e.GetType()}\nMessage: {e.Message}\nStack Trace: {e.StackTrace}";

            Console.WriteLine(message);
        }

        public void WriteDebug(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteLog(string message)
        {
            WriteDebug(message);
        }
    }
}

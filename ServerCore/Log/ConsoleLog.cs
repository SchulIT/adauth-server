using System;
using System.Diagnostics;

namespace ServerCore.Log
{
    public class ConsoleLog : ILog
    {
        public void Initialise() { }

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

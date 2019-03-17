using System;
using System.Diagnostics;

namespace ServerCore.Log
{
    /// <summary>
    /// Logs messages into the Windows EventLog
    /// </summary>
    public class EventLogger : ILogger
    {
        private const string LogSource = "AD Auth Server";
        private const string LogName = "Auth Server";

        public void Initialize()
        {
            try
            {
                if (!EventLog.SourceExists(LogSource))
                {
                    WriteDebug("Create EventLog source");

                    try
                    {
                        EventLog.CreateEventSource(LogSource, LogName);
                    }
                    catch(Exception e)
                    {
                        WriteDebug($"EventLog source was not created due to an error: {e.Message}");
                    }
                }
                else
                {
                    WriteDebug("EventLog source already exists");
                }
            }
            catch { }
        }

        public void WriteException(Exception e)
        {
            var message = $"Type: {e.GetType()}\nMessage: {e.Message}\nStack Trace: {e.StackTrace}";

            EventLog.WriteEntry(LogSource, message, EventLogEntryType.Error);
        }

        public void WriteDebug(string message) { }

        public void WriteLog(string message)
        {
            EventLog.WriteEntry(LogSource, message, EventLogEntryType.Information);
        }
    }
}

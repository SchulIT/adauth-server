using System;
using System.Diagnostics;

namespace ServerCore.Log
{
    public class EventLog : ILog
    {
        private const string LOG_SOURCE = "AD Auth Server";
        private const string LOG_NAME = "Auth Server";

        public void Initialise()
        {
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists(LOG_SOURCE))
                {
                    WriteDebug("Lege EventLog an");

                    try
                    {
                        System.Diagnostics.EventLog.CreateEventSource(LOG_SOURCE, LOG_NAME);
                    }
                    catch
                    {
                        WriteDebug("Konnte EventLog nicht anlegen");
                    }
                }
                else
                {
                    WriteDebug("Event-Log existiert");
                }
            }
            catch { }
        }

        public void WriteException(Exception e)
        {
            var message = $"Type: {e.GetType()}\nMessage: {e.Message}\nStack Trace: {e.StackTrace}";

            System.Diagnostics.EventLog.WriteEntry(LOG_SOURCE, message, EventLogEntryType.Error);
        }

        public void WriteDebug(string message) { }

        public void WriteLog(string message)
        {
            System.Diagnostics.EventLog.WriteEntry(LOG_SOURCE, message, EventLogEntryType.Information);
        }
    }
}

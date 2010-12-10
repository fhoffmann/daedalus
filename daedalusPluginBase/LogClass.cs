using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace daedalusPluginBase
{
    public class LogEntry
    {
        public enum LogLevel
        {
            Verbose, Information, Warning, Error
        }

        string msg = null, title = null;
        LogLevel level = LogLevel.Verbose;
        DateTime timestamp = DateTime.Now;
        public string Message { get { return this.msg; } }
        public string Title { get { return this.title; } }
        public LogLevel Level { get { return this.level; } }
        public DateTime Timestamp { get { return this.timestamp; } }

        public LogEntry(string _title, string _msg)
        {
            this.msg = _msg;
            this.title = _title;
            this.timestamp = DateTime.Now;
        }
        public LogEntry(string _title, string _msg, LogLevel _level)
        {
            this.level = _level;
            this.msg = _msg;
            this.title = _title;
            this.timestamp = DateTime.Now;
        }
    }

    public static class LogClass
    {
        /// <summary>
        /// Liste aller Log-Einträge, die zur Laufzeit eingegangen sind.
        /// </summary>
        private static List<LogEntry> listOfLogEntries = new List<LogEntry>();

        private static Queue<LogEntry> logentries = new Queue<LogEntry>();
        private static System.Threading.Timer timer = new System.Threading.Timer(new System.Threading.TimerCallback(Retry));

        public static LogEntry[] GetAllLogEntries()
        {
            return listOfLogEntries.ToArray();
        }

        public static void LogIt(LogEntry le)
        {
            if (le != null)
            {
                listOfLogEntries.Add(le);
                logentries.Enqueue(le);
                evt_newLogMessageMethod();
            }
        }

        public delegate void evt_newLogMessageHandle(LogEntry le);
        public static event evt_newLogMessageHandle evt_newLogMessage;
        private static void evt_newLogMessageMethod()
        {
            if (evt_newLogMessage != null)
            {
                while (logentries.Count > 0)
                {
                    evt_newLogMessage(logentries.Dequeue());
                }
            }
            else
            {
                // Nachrichten konnten nicht rausgehauen werden, timer starten und es erneut versuchen
                timer.Change(2000, System.Threading.Timeout.Infinite);
            }
        }

        private static void Retry(Object stateInfo)
        {
            evt_newLogMessageMethod();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace LigthScadaClient.Logic
{
    public class LogMessage
    {
        public string Message;
        public DateTime Timestamp;

        public string Log => string.Join(string.Empty, new string[] { Timestamp.ToString("HH:mm:ss"), ": ", Message, "\n" });
    }

    public class StatusLogger
    {
        private static StatusLogger m_instance;
        private TextBlock m_console;
        private List<LogMessage> m_messages = new List<LogMessage>();

        public static StatusLogger Instance => m_instance;

        public StatusLogger(TextBlock destination)
        {
            m_console = destination;
            m_instance = this;
        }

        public void Log(string message)
        {
            AddMessage(message);
            m_console.Dispatcher.Invoke(() => m_console.Text = GetLog());
        }

        private void AddMessage(string message)
        {
            m_messages.Add(new LogMessage
            {
                Message = message,
                Timestamp = DateTime.Now
            });
            while (m_messages.Count > 50)
                m_messages.RemoveAt(0);
        }

        private string GetLog()
        {
            string log = string.Join(string.Empty, m_messages.Select(x => x.Log).Reverse());
            return log;
        }
    }
}
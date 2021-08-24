using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ModbusSimulator
{
    public class LogMessage
    {
        public string Message;
        public DateTime Timestamp;

        public string Log => String.Join(String.Empty, new String[] { Timestamp.ToString("HH:mm:ss"), ": ", Message, "\n" });
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
            Dispatcher.CurrentDispatcher.Invoke(() => m_console.Text = GetLog());
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
            string log = String.Join(String.Empty, m_messages.Select(x => x.Log).Reverse());
            return log;
        }
    }
}
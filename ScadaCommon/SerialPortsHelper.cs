using System.IO.Ports;

namespace ScadaCommon
{
    public class SerialPortsHelper
    {
        /// <summary>
        /// Determines whether a COM Port is open or closed
        /// </summary>
        /// <param name="portName">COM Port name</param>
        /// <returns>true if open, false otherwise</returns>
        public static bool CheckIfPortIsOpen(string portName)
        {
            try
            {
                var port = new SerialPort(portName);
                port.Open();
                port.Close();
                port.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

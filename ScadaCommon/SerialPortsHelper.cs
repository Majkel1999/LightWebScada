using System.IO.Ports;

namespace ScadaCommon
{
    public class SerialPortsHelper
    {
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

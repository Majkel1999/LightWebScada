using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using LigthScadaClient.Logic.Utility;
using Newtonsoft.Json;
using ScadaCommon;

namespace LigthScadaClient.Logic
{
    public class LocalConfiguration : Singleton<LocalConfiguration>
    {
        private const string ConfigFileName = "config.cfg";

        public int ClientId = 1;
        public string ApiKey;
        public string COMPort;
        public string IP;
        public int TCPPort;
        public int SlaveID = 1;
        public int Baudrate = 9600;
        public int Interval = 5;
        public Parity Parity = Parity.None;
        public StopBits StopBits = StopBits.Two;

        private ClientConfig m_config;

        [JsonIgnore] public DataSet DataSet => m_config.Registers;
        [JsonIgnore] public string ConfigurationName => m_config.Name;
        [JsonIgnore] public bool IsTCP => m_config.Protocol == Protocol.ModbusTCP;

        public void SaveConfiguration() => SaveConfigToFile();

        public void SetConfiguration(ClientConfigEntity configEntity)
        {
            m_config = JsonConvert.DeserializeObject<ClientConfig>(configEntity.ConfigJson);
        }

        public string IsConfigurationCorrect()
        {
            if (IsTCP)
            {
                if (TCPPort < 0 || TCPPort > 65535)
                    return "TCP Port not in range 0-65535";

            }
            else
            {
                if (SerialPort.GetPortNames().Any(x => x == COMPort))
                {
                    if (!SerialPortsHelper.CheckIfPortIsOpen(COMPort))
                        return $"{COMPort} is already open!";
                }
            }
            return null;
        }

        protected override void OnCreate()
        {
            LoadConfigFromFile();
        }

        private void LoadConfigFromFile()
        {
            if (File.Exists(ConfigFileName))
            {
                using StreamReader theReader = new StreamReader(ConfigFileName, Encoding.UTF8);
                string dataAsJson = theReader.ReadToEnd();
                theReader.Close();
                JsonConvert.PopulateObject(dataAsJson, this);
            }
        }

        private void SaveConfigToFile()
        {
            string dataAsJson = JsonConvert.SerializeObject(this, Formatting.Indented);
            using (FileStream file = File.Create(ConfigFileName))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(dataAsJson);
                file.Write(info, 0, info.Length);
            }
        }
    }
}

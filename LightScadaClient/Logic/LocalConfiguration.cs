using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using LigthScadaClient.Logic.Utility;
using Newtonsoft.Json;
using ScadaCommon;

namespace LigthScadaClient.Logic
{
    public class LocalConfiguration : Singleton<LocalConfiguration>, INotifyPropertyChanged
    {
        private const string ConfigFileName = "config.cfg";

        private ClientConfig m_config;
        private int m_clientId = 1;
        private string m_ApiKey;
        private string m_ComPort;
        private string m_Ip;
        private int m_TcpPort;
        private int m_SlaveID = 1;
        private int baudrate = 9600;
        private int interval = 5;
        private Parity parity = Parity.None;
        private StopBits stopBits = StopBits.Two;

        public event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore] public DataSet DataSet => m_config.Registers;
        [JsonIgnore] public bool IsTCP => m_config != null && m_config.Protocol == Protocol.ModbusTCP;
        [JsonIgnore] public bool IsNotTCP => m_config != null && m_config.Protocol != Protocol.ModbusTCP;

        public int ClientId { get => m_clientId; set { m_clientId = value; OnPropertyChanged("ClientId"); } }
        public string ApiKey { get => m_ApiKey; set { m_ApiKey = value; OnPropertyChanged("ApiKey"); } }
        public string COMPort { get => m_ComPort; set { m_ComPort = value; OnPropertyChanged("COMPort"); } }
        public string IP { get => m_Ip; set { m_Ip = value; OnPropertyChanged("IP"); } }
        public int TCPPort { get => m_TcpPort; set { m_TcpPort = value; OnPropertyChanged("TCPPort"); } }
        public int SlaveID { get => m_SlaveID; set { m_SlaveID = value; OnPropertyChanged("SlaveID"); } }
        public int Baudrate { get => baudrate; set { baudrate = value; OnPropertyChanged("Baudrate"); } }
        public int Interval { get => interval; set { interval = value; OnPropertyChanged("Interval"); } }
        public Parity Parity { get => parity; set { parity = value; OnPropertyChanged("Parity"); } }
        public StopBits StopBits { get => stopBits; set { stopBits = value; OnPropertyChanged("StopBits"); } }

        public void SaveConfiguration() => SaveConfigToFile();

        public void SetConfiguration(ClientConfigEntity configEntity)
        {
            m_config = JsonConvert.DeserializeObject<ClientConfig>(configEntity.ConfigJson);
            OnPropertyChanged(string.Empty);
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
                OnPropertyChanged(string.Empty);
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

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

using DatabaseClasses;
using DataRegisters;
using LigthScadaClient.Logic.Utility;
using Newtonsoft.Json;

namespace LigthScadaClient.Logic
{
    public class LocalConfiguration : Singleton<LocalConfiguration>
    {
        private string m_name = "Testowy";
        private string m_apiKey;
        private string m_configName;
        private Protocol m_protocol;
        private DataSet m_dataSet;
        private string m_comPort = "COM4";
        private string m_ip;
        private int m_port;

        public DataSet DataSet => m_dataSet;
        public string ApiKey => m_apiKey;
        public string Name => m_name;
        public string ConfigurationName => m_configName;
        public string COMPort => m_comPort;
        public string IP => m_ip;
        public int Port => m_port;
        public bool IsTCP => m_protocol == Protocol.ModbusTCP;

        public void SetConfiguration(ClientConfigEntity configEntity)
        {
            ClientConfig config = JsonConvert.DeserializeObject<ClientConfig>(configEntity.ConfigJson);
            m_configName = config.Name;
            m_protocol = config.Protocol;
            m_dataSet = config.Registers;
        }

        public void SetApiKey(string apiKey)
        {
            m_apiKey = apiKey;
        }
    }
}

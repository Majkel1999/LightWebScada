﻿using System.IO;
using System.IO.Ports;
using System.Text;
using DatabaseClasses;
using DataRegisters;
using LigthScadaClient.Logic.Utility;
using Newtonsoft.Json;

namespace LigthScadaClient.Logic
{
    public class LocalConfiguration : Singleton<LocalConfiguration>
    {
        private const string FileName = "config.cfg";

        public string Name;
        public string ApiKey;
        public string COMPort;
        public string IP;
        public int TCPPort;
        public int SlaveID = 1;
        public int Baudrate = 9600;
        public Parity Parity = Parity.None;
        public StopBits StopBits = StopBits.Two;

        [JsonProperty] private ClientConfig m_config;

        [JsonIgnore] public DataSet DataSet => m_config.Registers;
        [JsonIgnore] public string ConfigurationName => m_config.Name;
        [JsonIgnore] public bool IsTCP => m_config.Protocol == Protocol.ModbusTCP;

        public void SaveConfiguration() => SaveConfigToFile();

        public void SetConfiguration(ClientConfigEntity configEntity)
        {
            m_config = JsonConvert.DeserializeObject<ClientConfig>(configEntity.ConfigJson);
        }

        protected override void OnCreate()
        {
            LoadConfigFromFile();
        }

        private void LoadConfigFromFile()
        {
            if (File.Exists(FileName))
            {
                using StreamReader theReader = new StreamReader(FileName, Encoding.UTF8);
                string dataAsJson = theReader.ReadToEnd();
                theReader.Close();
                JsonConvert.PopulateObject(dataAsJson, this);
            }
        }

        private void SaveConfigToFile()
        {
            string dataAsJson = JsonConvert.SerializeObject(this, Formatting.Indented);
            using (FileStream file = File.Create(FileName))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(dataAsJson);
                file.Write(info, 0, info.Length);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ScadaCommon;

namespace FrontEnd.Areas.Organizations.Data
{
    public class ViewElement
    {
        public ViewType ViewType;

        [JsonProperty] private RegisterType m_registerType;
        [JsonProperty] private int m_registerAddress;
        [JsonProperty] private int m_clientId = 1;
        [JsonIgnore] private RegisterFrame m_frameValue;
        [JsonIgnore] private bool m_updated = false;
        [JsonIgnore] private List<RegisterFrame> m_initialData;

        [JsonIgnore] public RegisterType RegisterType => m_registerType;
        [JsonIgnore] public int RegisterAddress => m_registerAddress;
        [JsonIgnore] public int Value => m_frameValue?.Value ?? 0;
        [JsonIgnore] public DateTime TimeStamp => m_frameValue?.Timestamp ?? DateTime.Now;
        [JsonIgnore] public int ClientId => m_clientId;
        [JsonIgnore] public List<RegisterFrame> InitialData => m_initialData;

        public void SetRegisterAddress(int address) => m_registerAddress = address;
        public void SetRegisterType(RegisterType registerType) => m_registerType = registerType;
        public void SetClientId(int id) => m_clientId = id;

        public void SetValue(RegisterFrame value)
        {
            if (m_frameValue != null && m_frameValue.Timestamp == value.Timestamp)
                return;
            m_frameValue = value;
            m_updated = true;
        }

        public int? GetValue()
        {
            if (m_updated)
            {
                m_updated = false;
                return Value;
            }
            return null;
        }

        public void SetInitialData(List<RegisterFrame> dataSets)
        {
            m_initialData = dataSets;
            m_frameValue = dataSets.OrderByDescending(x=>x.Timestamp).FirstOrDefault();
        }
    }
}
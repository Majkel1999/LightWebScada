using System.Collections.Generic;
using DatabaseClasses;
using DataRegisters;
using Newtonsoft.Json;

namespace FrontEnd.Areas.Organizations.Data
{
    public class ViewElement
    {
        public ViewType ViewType;

        [JsonProperty] private RegisterType m_registerType;
        [JsonProperty] private int m_registerAddress;
        [JsonIgnore] private int m_value;
        [JsonIgnore] private List<RegisterFrame> m_initialData;

        [JsonIgnore] public RegisterType RegisterType => m_registerType;
        [JsonIgnore] public int RegisterAddress => m_registerAddress;
        [JsonIgnore] public int Value => m_value;
        [JsonIgnore] public List<RegisterFrame> InitialData => m_initialData;

        public void SetRegisterAddress(int address) => m_registerAddress = address;
        public void SetRegisterType(RegisterType registerType) => m_registerType = registerType;

        public void SetValue(int value) => m_value = value;
        public void SetValue(bool value) => m_value = value ? 1 : 0;

        public void SetInitialData(List<RegisterFrame> dataSets)
        {
            m_initialData = dataSets;
        }
    }
}
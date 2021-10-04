using DataRegisters;
using Newtonsoft.Json;

namespace FrontEnd.Areas.Organizations.Data
{
    public class ViewElement
    {
        public ViewType ViewType;

        [JsonProperty] private RegisterType m_registerType;
        [JsonProperty] private int m_registerAddress;
        private int m_value;

        [JsonIgnore] public RegisterType RegisterType => m_registerType;
        [JsonIgnore] public int RegisterAddress => m_registerAddress;
        [JsonIgnore] public int Value => m_value;

        public void SetRegisterAddress(int address) => m_registerAddress = address;
        public void SetRegisterType(RegisterType registerType) => m_registerType = registerType;

        public void SetValue(int value) => m_value = value;
        public void SetValue(bool value) => m_value = value ? 1 : 0;
    }
}
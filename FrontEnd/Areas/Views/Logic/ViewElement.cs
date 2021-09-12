using DataRegisters;
using Newtonsoft.Json;

namespace FrontEnd.Areas.Organizations.Data
{
    public abstract class ViewElement
    {
        [JsonProperty]
        protected DataType m_dataType;
        [JsonProperty]
        protected RegisterType m_registerType;
        [JsonProperty]
        protected Register m_register;
        [JsonProperty]
        protected ViewType m_viewType;

        [JsonIgnore]
        public DataType DataType => m_dataType;
        [JsonIgnore]
        public ViewType ViewType => m_viewType;
        [JsonIgnore]
        public RegisterType RegisterType => m_registerType;
        [JsonIgnore]
        public int RegisterAddress => m_register.RegisterAddress;

        public ViewElement(Register register, RegisterType registerType, ViewType viewType)
        {
            m_viewType = viewType;
            m_register = register;
            m_registerType = registerType;
            if (m_registerType == RegisterType.CoilRegister || m_registerType == RegisterType.DiscreteInput)
                m_dataType = DataType.Boolean;
            else
                m_dataType = DataType.Integer;
        }

        public abstract void UpdateData(Register register);

        public void SetRegisterType(RegisterType registerType) => m_registerType = registerType;
        public void SetRegisterAddress(int address) => m_register.RegisterAddress = address;
    }
}
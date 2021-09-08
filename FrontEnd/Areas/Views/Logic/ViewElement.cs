using DataRegisters;

namespace FrontEnd.Areas.Organizations.Data
{
    public abstract class ViewElement
    {
        protected DataType m_dataType;
        protected RegisterType m_registerType;
        protected Register m_register;
        protected ViewType m_viewType;

        public DataType DataType => m_dataType;
        public ViewType ViewType => m_viewType;
        public RegisterType RegisterType => m_registerType;
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
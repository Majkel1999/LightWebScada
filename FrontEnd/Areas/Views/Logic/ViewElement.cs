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

        public ViewElement(Register register, RegisterType type)
        {
            m_register = register;
            m_registerType = type;
            if (m_registerType == RegisterType.CoilRegister || m_registerType == RegisterType.DiscreteInput)
                m_dataType = DataType.Boolean;
            else
                m_dataType = DataType.Integer;
        }

        public abstract void UpdateData(Register register);
    }
}
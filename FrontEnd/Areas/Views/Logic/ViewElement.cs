using DataRegisters;

namespace FrontEnd.Areas.Organizations.Data
{
    public abstract class ViewElement
    {
        protected DataType m_dataType;
        protected RegisterType m_registerType;
        protected Register m_register;

        public DataType DataType => m_dataType;

        public ViewElement(Register register, RegisterType type)
        {
            m_register = register;
            m_registerType = type;
        }

        public abstract void UpdateData(Register register);
    }
}
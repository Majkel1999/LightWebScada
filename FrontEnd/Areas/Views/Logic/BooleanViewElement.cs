using DataRegisters;

namespace FrontEnd.Areas.Organizations.Data
{
    public class BooleanViewElement : ViewElement
    {
        private bool m_value;

        public bool Value => m_value;

        public BooleanViewElement(Register register, RegisterType registerType, ViewType viewType) : base(register, registerType, viewType)
        {
            if (m_registerType != RegisterType.CoilRegister && m_registerType != RegisterType.DiscreteInput)
                throw new System.Exception("Wrong RegisterType in BooleanViewElement");
        }

        public override void UpdateData(Register register)
        {
            DiscreteRegister discreteRegister = register as DiscreteRegister;
            m_value = discreteRegister.CurrentValue;
        }

        public void UpdateData(bool value)
        {
            m_value = value;
        }
    }
}
using DataRegisters;

namespace FrontEnd.Areas.Organizations.Data
{
    public class BooleanViewElement : ViewElement
    {
        private bool m_value;

        public bool Value => m_value;

        public BooleanViewElement(Register register, RegisterType type) : base(register, type)
        {
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
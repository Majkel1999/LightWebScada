using DataRegisters;

namespace FrontEnd.Areas.Organizations.Data
{
    public class IntegerViewElement : ViewElement
    {
        private int m_value;

        public int Value => m_value;

        public IntegerViewElement(Register register, RegisterType type) : base(register, type)
        {
        }

        public override void UpdateData(Register register)
        {
            ValueRegister valueRegister = register as ValueRegister;
            m_value = valueRegister.CurrentValue;
        }

        public void UpdateData(int value)
        {
            m_value = value;
        }
    }
}
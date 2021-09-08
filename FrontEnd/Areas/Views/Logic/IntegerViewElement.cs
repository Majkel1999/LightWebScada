using DataRegisters;

namespace FrontEnd.Areas.Organizations.Data
{
    public class IntegerViewElement : ViewElement
    {
        private int m_value;

        public int Value => m_value;

        public IntegerViewElement(ValueRegister register, RegisterType registerType, ViewType viewType) : base(register, registerType, viewType)
        {
            if (m_registerType != RegisterType.InputRegister && m_registerType != RegisterType.HoldingRegister)
                throw new System.Exception("Wrong RegisterType in IntegerViewElement");
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
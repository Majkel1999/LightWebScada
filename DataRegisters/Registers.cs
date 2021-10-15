namespace DataRegisters
{
    public enum RegisterType
    {
        CoilRegister,
        DiscreteInput,
        InputRegister,
        HoldingRegister
    }

    public class Register
    {
        public int CurrentValue { get; set; }
        public int RegisterAddress { get; set; }
    }
}

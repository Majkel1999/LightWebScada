namespace DataRegisters
{
    public enum RegisterType
    {
        CoilRegister,
        DiscreteInput,
        InputRegister,
        HoldingRegister
    }

    public abstract class Register
    {
        public int RegisterNumber { get; set; }
    }

    public class DiscreteRegister : Register
    {
        public bool CurrentValue { get; set; }
    }

    public class ValueRegister : Register
    {
        public int CurrentValue { get; set; }
    }
}

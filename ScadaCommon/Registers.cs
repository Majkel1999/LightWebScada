namespace ScadaCommon
{
    /// <summary>
    /// Type of Modbus register
    /// </summary>
    public enum RegisterType
    {
        CoilRegister,
        DiscreteInput,
        InputRegister,
        HoldingRegister
    }
    /// <summary>
    /// Represents a value register
    /// </summary>
    public class Register
    {
        public int CurrentValue { get; set; }
        public int RegisterAddress { get; set; }
    }
}

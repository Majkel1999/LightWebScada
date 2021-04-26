namespace LigthScadaClient.DataModels
{
    public class Register
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

namespace DataRegisters
{
    public class ClientConfig
    {
        public string Name { get; set; }
        public Protocol Protocol { get; set; }
        public DataSet Registers { get; set; } = new DataSet();
    }
}

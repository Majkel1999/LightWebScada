using System.Collections.Generic;

namespace DataRegisters
{
    public class DataSet
    {
        public List<DiscreteRegister> CoilRegisters { get; set; }
        public List<DiscreteRegister> DiscreteInputs { get; set; }
        public List<ValueRegister> InputRegisters { get; set; }
        public List<ValueRegister> HoldingRegisters { get; set; }
    }
}

using System.Collections.Generic;

namespace DataRegisters
{
    public class DataSet
    {
        public List<DiscreteRegister> CoilRegisters { get; set; } = new List<DiscreteRegister>();
        public List<DiscreteRegister> DiscreteInputs { get; set; } = new List<DiscreteRegister>();
        public List<ValueRegister> InputRegisters { get; set; } = new List<ValueRegister>();
        public List<ValueRegister> HoldingRegisters { get; set; } = new List<ValueRegister>();
    }
}

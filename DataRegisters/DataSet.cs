using System.Collections.Generic;
using System.Linq;

namespace DataRegisters
{
    public class DataSet
    {
        public List<Register> CoilRegisters { get; set; } = new List<Register>();
        public List<Register> DiscreteInputs { get; set; } = new List<Register>();
        public List<Register> InputRegisters { get; set; } = new List<Register>();
        public List<Register> HoldingRegisters { get; set; } = new List<Register>();

        public bool Contains(int registerAddress, RegisterType type)
        {
            switch (type)
            {
                case RegisterType.CoilRegister:
                    return CoilRegisters.Any(x => x.RegisterAddress == registerAddress);
                case RegisterType.DiscreteInput:
                    return DiscreteInputs.Any(x => x.RegisterAddress == registerAddress);
                case RegisterType.InputRegister:
                    return InputRegisters.Any(x => x.RegisterAddress == registerAddress);
                case RegisterType.HoldingRegister:
                    return HoldingRegisters.Any(x => x.RegisterAddress == registerAddress);
                default:
                    return false;
            }
        }

        public int GetValue(int registerAddress, RegisterType type)
        {
            switch (type)
            {
                case RegisterType.CoilRegister:
                    return CoilRegisters.Where(x => x.RegisterAddress == registerAddress).First().CurrentValue;
                case RegisterType.DiscreteInput:
                    return DiscreteInputs.Where(x => x.RegisterAddress == registerAddress).First().CurrentValue;
                case RegisterType.InputRegister:
                    return InputRegisters.Where(x => x.RegisterAddress == registerAddress).First().CurrentValue;
                case RegisterType.HoldingRegister:
                    return HoldingRegisters.Where(x => x.RegisterAddress == registerAddress).First().CurrentValue;
                default:
                    return 0;
            }
        }

        public List<List<Register>> GetRegisters()
        {
            return new List<List<Register>>() {
                CoilRegisters,DiscreteInputs,InputRegisters,HoldingRegisters
            };
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace DataRegisters
{
    public class DataSet
    {
        public List<DiscreteRegister> CoilRegisters { get; set; } = new List<DiscreteRegister>();
        public List<DiscreteRegister> DiscreteInputs { get; set; } = new List<DiscreteRegister>();
        public List<ValueRegister> InputRegisters { get; set; } = new List<ValueRegister>();
        public List<ValueRegister> HoldingRegisters { get; set; } = new List<ValueRegister>();

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
                    return CoilRegisters.Where(x => x.RegisterAddress == registerAddress).First().CurrentValue.ToInt();
                case RegisterType.DiscreteInput:
                    return DiscreteInputs.Where(x => x.RegisterAddress == registerAddress).First().CurrentValue.ToInt();
                case RegisterType.InputRegister:
                    return InputRegisters.Where(x => x.RegisterAddress == registerAddress).First().CurrentValue;
                case RegisterType.HoldingRegister:
                    return HoldingRegisters.Where(x => x.RegisterAddress == registerAddress).First().CurrentValue;
                default:
                    return 0;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace ScadaCommon
{
    /// <summary>
    /// Represents one frame of data received from local clients
    /// </summary>
    public class DataSet
    {
        public List<Register> CoilRegisters { get; set; } = new List<Register>();
        public List<Register> DiscreteInputs { get; set; } = new List<Register>();
        public List<Register> InputRegisters { get; set; } = new List<Register>();
        public List<Register> HoldingRegisters { get; set; } = new List<Register>();

        public List<List<Register>> GetRegisters()
        {
            return new List<List<Register>>() {
                CoilRegisters,DiscreteInputs,InputRegisters,HoldingRegisters
            };
        }
    }
}

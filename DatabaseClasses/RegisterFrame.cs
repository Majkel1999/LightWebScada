using System;
using DataRegisters;

namespace DatabaseClasses
{
    public class RegisterFrame
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime Timestamp { get; set; }
        public RegisterType RegisterType { get; set; }
        public int RegisterAddress { get; set; }
        public int Value { get; set; }
    }
}
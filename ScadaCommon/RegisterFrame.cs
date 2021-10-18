using System;

namespace ScadaCommon
{
    /// <summary>
    /// Represents a single frame of a single register
    /// </summary>
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
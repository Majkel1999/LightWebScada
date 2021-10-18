using System;

namespace ScadaCommon
{
    public class DataFrame
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Dataset { get; set; }
    }
}
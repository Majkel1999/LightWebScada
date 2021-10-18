using System;

namespace ScadaCommon
{
    /// <summary>
    /// Entity for sending data to API server from the local app
    /// </summary>
    public class DataFrame
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Dataset { get; set; }
    }
}
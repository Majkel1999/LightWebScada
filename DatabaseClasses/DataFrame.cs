using System;

namespace DatabaseClasses
{
    public class DataFrame
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public string Dataset { get; set; }
    }
}
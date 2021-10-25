namespace ScadaCommon
{
    /// <summary>
    /// Represents a config entity, saved in database.
    /// Configuration instance is saved as a serialized ClientConfig instance.
    /// </summary>
    public class ClientConfigEntity
    {
        public int ID { get; set; }
        public string ConfigName { get; set; }
        public string ConfigJson { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}

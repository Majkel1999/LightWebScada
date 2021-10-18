namespace ScadaCommon
{
    /// <summary>
    /// Represents a single organization entity
    /// </summary>
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string ApiKey { get; set; }
    }
}

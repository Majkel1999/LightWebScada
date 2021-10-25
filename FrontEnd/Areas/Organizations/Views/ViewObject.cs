using ScadaCommon;

namespace FrontEnd.Areas.Organizations.Data
{
    public class ViewObject
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public string ViewJson { get; set; }
        public string Name { get; set; }
    }
}
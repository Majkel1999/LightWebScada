using System.ComponentModel.DataAnnotations;

namespace FrontEnd.DataModels
{
    public class MemberModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool Admin { get; set; }
        public bool CanShowViews { get; set; }
        public bool CanEditViews { get; set; }
        public bool CanDeleteViews { get; set; }
        public bool CanEditConfigurations { get; set; }
        public bool CanDeleteConfigurations { get; set; }
    }
}

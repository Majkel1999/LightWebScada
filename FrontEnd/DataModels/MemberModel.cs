using System.ComponentModel.DataAnnotations;

namespace FrontEnd.DataModels
{
    public class MemberModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool Admin
        {
            get => m_isAdmin;
            set
            {
                m_isAdmin = value;
                CanShowViews = true;
                CanEditViews = value;
                CanEditConfigurations = value;
                CanCreateReports = value;
            }
        }
        public bool CanShowViews { get; set; }
        public bool CanEditViews { get; set; }
        public bool CanEditConfigurations { get; set; }
        public bool CanCreateReports { get; set; }

        private bool m_isAdmin = false;
    }
}

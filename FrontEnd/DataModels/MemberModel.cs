using System.ComponentModel.DataAnnotations;
namespace FrontEnd.DataModels
{
    public class MemberModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        [Required]
        public bool Admin { get; set; }
    }
}

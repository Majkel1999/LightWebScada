using System.ComponentModel.DataAnnotations;

namespace FrontEnd.DataModels
{
    public class OrganizationRegistrationModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [DataType(DataType.Text)]
        [MinLength(3,ErrorMessage = "Name must be longer than 3 characters!")]
        [MaxLength(80,ErrorMessage = "Name cannot be longer than 80 characters!")]
        public string OrganizationName { get; set; }
    }
}

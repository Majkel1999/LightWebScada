using System.ComponentModel.DataAnnotations;

namespace FrontEnd.DataModels
{
    public class OrganizationRegistrationModel
    {
        [Required(ErrorMessage = "Nazwa organizacji jest wymagana!")]
        [DataType(DataType.Text)]
        [MinLength(3,ErrorMessage = "Nazwa organizacji musi być dłuższa niż 3 znaki!")]
        [MaxLength(80,ErrorMessage = "Nazwa organizacji nie może być dłuższa niż 80 znaków!")]
        public string OrganizationName { get; set; }
    }
}

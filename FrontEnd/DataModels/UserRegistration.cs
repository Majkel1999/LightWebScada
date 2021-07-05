using System.ComponentModel.DataAnnotations;
namespace FrontEnd.DataModels
{
    public class UserRegistration
    {
        [Required(ErrorMessage = "Pole jest wymagane!")]
        [StringLength(20, ErrorMessage = "Nazwa użytkownika jest za długa!")]
        [MinLength(6, ErrorMessage = "Nazwa użytkownika musi mieć conajmniej 6 znaków!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasła nie zgadzają się!")]
        public string ConfirmPassword { get; set; }
    }
}
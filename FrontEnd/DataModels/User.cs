using System.ComponentModel.DataAnnotations;

namespace FrontEnd.DataModels
{
    public class User
    {
        [Required(ErrorMessage = "Musisz podać login aby się zalogować!")]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        [Required(ErrorMessage ="Musisz podać hasło aby się zalogować!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using DataRegisters;

namespace FrontEnd.DataModels
{
    public class ConfigValueModel
    {
        public RegisterType Type { get; set; }
        [Required]
        [Range(0, 9999, ErrorMessage = "Rejestr może być tylko z zakresu 0-9999")]
        public int RegisterNumber { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using ScadaCommon;

namespace FrontEnd.DataModels
{
    public class ConfigValueModel
    {
        public RegisterType Type { get; set; }
        [Required]
        [Range(0, 9999, ErrorMessage = "Values from range 0-9999")]
        public int RegisterNumber { get; set; }
    }
}

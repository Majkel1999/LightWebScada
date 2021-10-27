using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScadaCommon
{
    public class ReportContent
    {
        public List<ReportElement> Content { get; set; }

        public ReportContent()
        {
            Content = new List<ReportElement>();
        }
    }

    public class ReportElement
    {
        [Required]
        public RegisterType Type { get; set; }
        [Required]
        [Range(0, 9999, ErrorMessage = "Address must be between 0 and 9999")]
        public int Address { get; set; }
        [Required]
        [Range(1, 99, ErrorMessage = "Client ID must be between 1 and 99")]
        public int ClientID { get; set; } = 1;
    }
}
using System.Collections.Generic;

namespace ScadaCommon
{
    public class ReportContent
    {
        public List<ReportElement> Content { get; set; }
    }

    public class ReportElement
    {
        public RegisterType Type { get; set; }
        public int Adress { get; set; }
        public int ClientID { get; set; }
    }
}
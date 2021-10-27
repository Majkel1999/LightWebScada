using Microsoft.AspNetCore.Mvc;
using ScadaCommon;

namespace LightScadaAPI.Controllers
{
    [Route("report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportGenerator m_reportService;

        public ReportController(IReportGenerator reportGenerator)
        {
            m_reportService = reportGenerator;
        }

        [HttpPost]
        public IActionResult Get([FromQuery] int OrganizationId, [FromBody] ReportContent Content)
        {
            byte[] pdfFile = m_reportService.GenerateReport(OrganizationId, Content);
            if (pdfFile == null)
                return NotFound();
            return File(pdfFile, "application/octet-stream", "Report.html");
        }
    }
}
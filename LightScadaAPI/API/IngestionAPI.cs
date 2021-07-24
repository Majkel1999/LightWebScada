using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DatabaseClasses;
using LightScadaAPI.Contexts;
using System;
using System.Threading.Tasks;

namespace FrontEnd.Pages.API
{
    [Route("api/send")]
    [ApiController]
    public class IngestionAPI : ControllerBase
    {
        private readonly string m_connectionString;
        private readonly DatasetWriter m_datasetWriter;

        public IngestionAPI(IConfiguration configuration)
        {
            m_connectionString = configuration.GetConnectionString("UserContextConnection");
            m_datasetWriter = new DatasetWriter(m_connectionString);
        }

        [HttpPost]
        public async Task<ActionResult> GetConfigurations([FromQuery] string apiKey, [FromBody] DataFrame dataFrame)
        {
            try
            {
                if (await m_datasetWriter.WriteToDatabase(dataFrame, apiKey))
                    return Accepted(new[] { "DataFrame written to database" });
                else
                    return BadRequest("Wrong api key!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

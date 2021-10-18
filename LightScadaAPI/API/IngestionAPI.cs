using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ScadaCommon;
using LightScadaAPI.Contexts;

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
        public async Task<ActionResult> PostDataFrame([FromQuery] string apiKey, [FromBody] DataFrame dataFrame)
        {
            try
            {
                var result = await m_datasetWriter.WriteToDatabase(dataFrame, apiKey);
                if (result.Item1)
                    return Accepted(new[] { "DataFrame written to database" });
                else
                    return BadRequest(result.Item2);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

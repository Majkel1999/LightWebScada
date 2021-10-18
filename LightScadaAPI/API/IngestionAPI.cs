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

        /// <summary>
        /// POST Request handler, receives data from local clients and saves it after transforming to database
        /// </summary>
        /// <param name="apiKey">received api key</param>
        /// <param name="dataFrame">Json Serialized DataFrame from local client</param>
        /// <returns>BadRequest if failed, Accepted otherwise</returns>
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

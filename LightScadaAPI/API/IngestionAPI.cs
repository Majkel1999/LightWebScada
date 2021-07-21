using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DatabaseClasses;
using LightScadaAPI.Contexts;
using System;

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
        public ActionResult GetConfigurations(DataFrame dataFrame)
        {
            try
            {
                m_datasetWriter.WriteToDatabase(dataFrame);
                return Accepted();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

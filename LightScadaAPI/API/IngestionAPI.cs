using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using DatabaseClasses;
using DataRegisters;
using Npgsql;

namespace FrontEnd.Pages.API
{
    [Route("api/send")]
    [ApiController]
    public class IngestionAPI : ControllerBase
    {
        private string m_connectionString;

        public IngestionAPI(IConfiguration configuration)
        {
            m_connectionString = configuration.GetConnectionString("UserContextConnection");
        }

        [HttpPost]
        public ActionResult GetConfigurations(string apikey)
        {
            return NoContent();
        }
    }
}

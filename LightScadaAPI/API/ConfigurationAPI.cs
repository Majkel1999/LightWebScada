using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using DatabaseClasses;
using Npgsql;

namespace FrontEnd.Pages.API
{
    [Route("api/configurations")]
    [ApiController]
    public class ConfigurationAPI : ControllerBase
    {
        private readonly string m_connectionString;

        public ConfigurationAPI(IConfiguration configuration)
        {
            m_connectionString = configuration.GetConnectionString("UserContextConnection");
            
        }

        [HttpGet("{apikey}")]
        public async Task<ActionResult<List<ClientConfigEntity>>> GetConfigurations(string apikey)
        {
            List<ClientConfigEntity> configList;
            using IDbConnection db = new NpgsqlConnection(m_connectionString);
            int id = -1;
            string query = @"Select ""OrganizationId"" From common.organization Where ""ApiKey""='" + apikey + "'";
            var ids = (await db.QueryAsync<int>(query)).ToList();
            if (ids.Count > 0)
                id = ids.First();
            else
                return NoContent();
            query = @"Select * From common.clientconfigentity where ""OrganizationId""='" + id + "'";
            configList = (await db.QueryAsync<ClientConfigEntity>(query)).ToList();
            return configList;
        }
    }
}

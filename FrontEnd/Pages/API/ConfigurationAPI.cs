using FrontEnd.Areas.Organizations.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FrontEnd.Pages.API
{
    [Route("api/configurations")]
    [ApiController]
    public class ConfigurationAPI : ControllerBase
    {
        private OrganizationContext m_organizationContext;
        public ConfigurationAPI(OrganizationContext organizationContext)
        {
            m_organizationContext = organizationContext;
        }

        [HttpGet("{apikey}")]
        public async Task<ActionResult<List<ClientConfigEntity>>> GetConfigurations(string apikey) {
            return NoContent();
        }
    }
}

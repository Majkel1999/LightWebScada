using FrontEnd.Areas.Organizations.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
            var organization = await m_organizationContext.Organizations.FirstAsync(o => o.ApiKey == apikey);
            if(organization.ApiKey == apikey){
                var configurations = await m_organizationContext.Configurations.Where(c => c.OrganizationId == organization.OrganizationId).ToListAsync();
                return configurations;
            }
            return NoContent();
        }
    }
}

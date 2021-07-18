using FrontEnd.Areas.Organizations.Data;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace FrontEnd.DataHandlers
{
    public class ApiKeyGenerator
    {
        private OrganizationContext m_organizationContext;

        public ApiKeyGenerator(OrganizationContext organizationContext)
        {
            m_organizationContext = organizationContext;
        }

        public string GenerateNewApiKey()
        {
            string apiKey = "";
            do
            {
                var key = new byte[32];
                using (var generator = RandomNumberGenerator.Create())
                    generator.GetBytes(key);
                apiKey = Convert.ToBase64String(key);
            } while (m_organizationContext.Organizations.Where(x => x.ApiKey == apiKey).Any());
            return apiKey;
        }
    }
}

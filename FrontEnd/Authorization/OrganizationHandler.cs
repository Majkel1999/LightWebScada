using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Areas.Organizations.Data;

namespace FrontEnd.Authorization
{
    public class OrganizationRequirement : IAuthorizationRequirement
    {
    }

    public class OrganizationHandler : AuthorizationHandler<OrganizationRequirement>
    {
        private OrganizationContext m_organizationContext;

        public OrganizationHandler(OrganizationContext organizationContext)
        {
            m_organizationContext = organizationContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrganizationRequirement requirement)
        {
            string resource = context.Resource?.ToString();
            if (int.TryParse(resource, out int organizationId))
            {
                string userName = context.User.Identity.Name.Normalize();
                if (m_organizationContext.Members.Where(x => x.OrganizationId == organizationId && x.UserName == userName).Any())
                    context.Succeed(requirement);
                else if (m_organizationContext.Organizations.Where(x => x.OrganizationId == organizationId && x.Owner == userName).Any())
                    context.Succeed(requirement);
                else
                    context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
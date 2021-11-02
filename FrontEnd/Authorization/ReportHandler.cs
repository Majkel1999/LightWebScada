using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Areas.Organizations.Data;

namespace FrontEnd.Authorization
{
    public class CreateReportRequirement : IAuthorizationRequirement
    {
    }

    public class CreateReportHandler : AuthorizationHandler<CreateReportRequirement>
    {
        private OrganizationContext m_organizationContext;

        public CreateReportHandler(OrganizationContext organizationContext)
        {
            m_organizationContext = organizationContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreateReportRequirement requirement)
        {
            string resource = context.Resource?.ToString();
            if (int.TryParse(resource, out int organizationId))
            {
                string userName = context.User.Identity.Name.Normalize();

                if (context.User.FindFirst(x => x.Type == OrganizationClaims.CreateReportsClaim && x.Value == organizationId.ToString()) != null)
                    context.Succeed(requirement);
                else if (m_organizationContext.Members.Where(x => x.OrganizationId == organizationId && x.isAdmin && x.UserName == userName).Any())
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
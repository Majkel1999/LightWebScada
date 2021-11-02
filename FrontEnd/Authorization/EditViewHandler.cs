using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Areas.Organizations.Data;

namespace FrontEnd.Authorization
{
    public class EditViewRequirement : IAuthorizationRequirement
    {
    }

    public class EditViewHandler : AuthorizationHandler<EditViewRequirement>
    {
        private OrganizationContext m_organizationContext;

        public EditViewHandler(OrganizationContext organizationContext)
        {
            m_organizationContext = organizationContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EditViewRequirement requirement)
        {
            string resource = context.Resource?.ToString();
            if (int.TryParse(resource, out int organizationId))
            {
                string userName = context.User.Identity.Name.Normalize();
                if (context.User.FindFirst(x => x.Type == OrganizationClaims.EditViewClaim && x.Value == organizationId.ToString()) != null)
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
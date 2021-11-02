using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Areas.Organizations.Data;

namespace FrontEnd.Authorization
{
    public class ShowViewRequirement : IAuthorizationRequirement
    {
    }

    public class ShowViewsAuthorizeAttribute : AuthorizeAttribute
    {
    }

    public class ShowViewHandler : AuthorizationHandler<ShowViewRequirement>
    {
        private OrganizationContext m_organizationContext;

        public ShowViewHandler(OrganizationContext organizationContext)
        {
            m_organizationContext = organizationContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ShowViewRequirement requirement)
        {
            string resource = context.Resource?.ToString();
            if (int.TryParse(resource, out int viewId))
            {
                int organizationId = m_organizationContext.Views.Where(x => x.Id == viewId).Select(x => x.OrganizationId).FirstOrDefault();
                string userName = context.User.Identity.Name.Normalize();
                if (context.User.FindFirst(x => x.Type == OrganizationClaims.ShowViewClaim && x.Value == organizationId.ToString()) != null)
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
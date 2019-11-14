using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Tutorial.WebAPI.JWT.NickChapsas.Authorization
{
    public class WorForCompanyRequirement : IAuthorizationRequirement
    {
        public string DomainName;
        public WorForCompanyRequirement(string domainName)
        {
            this.DomainName = domainName;
        }
    }

    public class WorkForCompanyHandler : AuthorizationHandler<WorForCompanyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, WorForCompanyRequirement requirement)
        {

            string userEmail = context.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

            if (userEmail.EndsWith(requirement.DomainName))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;

        }
    }
}

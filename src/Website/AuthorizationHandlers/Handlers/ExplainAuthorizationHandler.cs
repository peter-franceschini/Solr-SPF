using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Models.Database;
using Website.Extensions;

namespace Website.Authorization.Handlers
{
    public class ExplainAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, SolrQueryResponseRecord>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, SolrQueryResponseRecord resource)
        {
            // If the resource isn't assigned to a specific user, allow
            if(resource.UserId == null)
            {
                context.Succeed(requirement);
            } // If the resource is assigned to a specific user, and it is this user
            else if (context.User.Identity.IsAuthenticated && context.User.GetUserId() == resource.UserId )
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
    

}

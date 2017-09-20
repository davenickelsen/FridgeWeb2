using System.Threading.Tasks;
using FridgeData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FridgeCoreWeb.Authorization
{
    public class UserAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, User>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
            User resource)
        {
            if (context.User != null)
            {
                context.Succeed(requirement); 
            }

            return Task.FromResult(0);
        }
    }
}

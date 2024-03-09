using Microsoft.AspNetCore.Authorization;
using userManagerApplication.Models;

namespace userManagerApplication.Indentity
{
    public class CustomAuthorizationHandler: AuthorizationHandler<CustomRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "role"))
            {
                var roles = context.User.FindAll("role").Select(r => r.Value);

                // Verificar si el usuario tiene el rol necesario para cumplir con la política
                if (roles.Contains(requirement.RequiredRole))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}

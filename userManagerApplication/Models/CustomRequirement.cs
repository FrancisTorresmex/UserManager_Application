using Microsoft.AspNetCore.Authorization;

namespace userManagerApplication.Models
{
    public class CustomRequirement: IAuthorizationRequirement
    {
        public string RequiredRole { get; }

        public CustomRequirement(string requiredRole)
        {
            RequiredRole = requiredRole;
        }
    }
}

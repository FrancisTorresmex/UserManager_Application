namespace userManagerApplication.Indentity
{
    /*
     * Add the required roles in the application here, and add them in the AddAuthorization() section of program.cs
     */

    public class IdentityData
    {
        public const string AdminUserClaimName = "Admin";
        public const string AdminUserPolicyName = "Admin";

        public const string UserClaimName = "User";
        public const string UserPolicyName = "User";
    }
}

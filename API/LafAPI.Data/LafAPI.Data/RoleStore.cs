namespace LafAPI.Data
{
    using System.Security.Claims;

    using LafAPI.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class RoleStore : RoleStore<Role, LafContext, string, IdentityUserRole<string>, IdentityRoleClaim<string>>
    {
        public RoleStore(LafContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }

        protected override IdentityRoleClaim<string> CreateRoleClaim(Role role, Claim claim) =>
            new IdentityRoleClaim<string>
                {
                    RoleId = role.Id,
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value,
                };
    }
}
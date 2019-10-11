namespace LafAPI.Web.Infrastructure.Extensions
{
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;

    using LafAPI.Data.Models;

    using Microsoft.AspNetCore.Identity;

    public static class UserManagerExtensions
    {
        public static async Task<GenericPrincipal> PrincipalResolver(this UserManager<User> userManager, string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null || user.IsDeleted)
            {
                return default;
            }

            var isValidPassword = await userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword)
            {
                return default;
            }

            var roles = await userManager.GetRolesAsync(user);
            var identity = new GenericIdentity(email, "Token");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            return new GenericPrincipal(identity, roles.ToArray());
        }

        public static GenericPrincipal PrincipalResolver(this UserManager<User> userManager, string email, string password, out User user)
        {
            user = userManager.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || user.IsDeleted)
            {
                return default;
            }

            var isValidPassword = userManager.CheckPasswordAsync(user, password).GetAwaiter().GetResult();
            if (!isValidPassword)
            {
                return default;
            }

            var roles = userManager.GetRolesAsync(user).GetAwaiter().GetResult();
            var identity = new GenericIdentity(email, "Token");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            return new GenericPrincipal(identity, roles.ToArray());
        }
    }
}
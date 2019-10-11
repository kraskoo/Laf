namespace LafAPI.Data
{
    using System.Security.Claims;

    using LafAPI.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class UserStore :
        UserStore<User,
            Role,
            LafContext,
            string,
            IdentityUserClaim<string>,
            IdentityUserRole<string>,
            IdentityUserLogin<string>,
            IdentityUserToken<string>,
            IdentityRoleClaim<string>>
    {
        public UserStore(LafContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }

        protected override IdentityUserRole<string> CreateUserRole(User user, Role role) =>
            new IdentityUserRole<string>
                {
                    RoleId = role.Id,
                    UserId = user.Id
                };

        protected override IdentityUserClaim<string> CreateUserClaim(User user, Claim claim)
        {
            var identityUserClaim = new IdentityUserClaim<string> { UserId = user.Id };
            identityUserClaim.InitializeFromClaim(claim);
            return identityUserClaim;
        }

        protected override IdentityUserLogin<string> CreateUserLogin(User user, UserLoginInfo login) =>
            new IdentityUserLogin<string>
                {
                    UserId = user.Id,
                    ProviderKey = login.ProviderKey,
                    LoginProvider = login.LoginProvider,
                    ProviderDisplayName = login.ProviderDisplayName
                };

        protected override IdentityUserToken<string> CreateUserToken(
            User user,
            string loginProvider,
            string name,
            string value) => new IdentityUserToken<string>
                                 {
                                     UserId = user.Id,
                                     LoginProvider = loginProvider,
                                     Name = name,
                                     Value = value
                                 };
    }
}
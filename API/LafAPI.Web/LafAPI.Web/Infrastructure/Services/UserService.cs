namespace LafAPI.Web.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using LafAPI.Data.Models;
    using LafAPI.Web.Infrastructure.Extensions;
    using LafAPI.Web.Infrastructure.Interfaces;
    using LafAPI.Web.Infrastructure.Middlewares.Auth;
    using LafAPI.Web.Infrastructure.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public class UserService : IUserService
    {
        public const string SuccessfullyAuthentication = "Successfully Authentication";
        public const string InvalidState = "Ivalid object state!";
        public const string InvalidEmail = "User with this userName doesn't exist!";
        public const string InvalidPassword = "Invalid Password!";
        private readonly UserManager<User> userManager;

        public UserService(UserManager<User> userManager) =>
            this.userManager = userManager;

        public async Task<LoginResponse> Authenticate(
            string email,
            string password,
            HttpContext httpContext)
        {
            var options = httpContext.RequestServices.GetRequiredService<IOptions<TokenProviderOptions>>().Value;
            var response = await httpContext.GetLoginResponse(
                               options,
                               await this.userManager.PrincipalResolver(email, password));
            response.Result = SuccessfullyAuthentication;
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.Result = InvalidEmail;
                return response;
            }

            var isValidPassword = await this.userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword)
            {
                response.Result = InvalidPassword;
                return response;
            }

            return response;
        }

        public LoginResponse Authenticate(string email, string password, HttpContext httpContext, out User user)
        {
            var options = httpContext.RequestServices.GetRequiredService<IOptions<TokenProviderOptions>>().Value;
            var response = httpContext
                .GetLoginResponse(
                    options,
                    this.userManager.PrincipalResolver(email, password, out user))
                .GetAwaiter()
                .GetResult();
            if (response == null)
            {
                return new LoginResponse { Result = InvalidState };
            }

            response.Result = SuccessfullyAuthentication;
            if (user == null)
            {
                response.Result = InvalidEmail;
                return response;
            }

            var isValidPassword = this.userManager
                .CheckPasswordAsync(user, password)
                .GetAwaiter()
                .GetResult();
            if (!isValidPassword)
            {
                response.Result = InvalidPassword;
                return response;
            }

            return response;
        }

        public IQueryable<User> UsersWithoutFriendshipWithCurrentUser(
            string userId,
            params Expression<Func<User, bool>>[] expressions) =>
            expressions.Aggregate(
                this.userManager.Users
                    .Include(u => u.UserFriends)
                    .Where(u => u.Id != userId)
                    .Where(u => u.UserFriends.All(uf => uf.FriendId != userId)),
                (current, expression) => current.Where(expression));

        public Task<bool> AnyAsync(
            Expression<Func<User, bool>> predicate,
            CancellationToken cancellationToken = default) =>
            this.userManager.Users.AnyAsync(predicate, cancellationToken);

        public Task<bool> AnyAsync(CancellationToken cancellationToken = default) =>
            this.userManager.Users.AnyAsync(cancellationToken);

        public Task<IdentityResult> CreateAsync(User user) =>
            this.userManager.CreateAsync(user);

        public Task<IdentityResult> CreateAsync(User user, string password) =>
            this.userManager.CreateAsync(user, password);

        public Task<IdentityResult> AddToRoleAsync(User user, string role) =>
            this.userManager.AddToRoleAsync(user, role);

        public Task<IList<User>> GetUsersInRoleAsync(string roleName) =>
            this.userManager.GetUsersInRoleAsync(roleName);

        public Task<IList<string>> GetRolesAsync(User user) =>
            this.userManager.GetRolesAsync(user);

        public Task<User> FindByIdAsync(string userId) =>
            this.userManager.FindByIdAsync(userId);

        public Task<User> FindByNameAsync(string userName) =>
            this.userManager.FindByNameAsync(userName);
    }
}
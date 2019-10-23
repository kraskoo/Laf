namespace LafAPI.Web.Infrastructure.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using LafAPI.Data.Models;
    using LafAPI.Web.Infrastructure.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        Task<IdentityResult> UploadAvatarImagePath(User user, string path);

        Task<LoginResponse> Authenticate(
            string email,
            string password,
            HttpContext httpContext);

        LoginResponse Authenticate(
            string email,
            string password,
            HttpContext httpContext,
            out User user);

        IQueryable<User> UsersWithoutFriendshipWithCurrentUser(
            string userId,
            params Expression<Func<User, bool>>[] expressions);

        Task<bool> AnyAsync(
            Expression<Func<User, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(CancellationToken cancellationToken = default);

        Task<IdentityResult> CreateAsync(User user);

        Task<IdentityResult> CreateAsync(User user, string password);

        Task<IdentityResult> AddToRoleAsync(User user, string role);

        Task<IList<User>> GetUsersInRoleAsync(string roleName);

        Task<IList<string>> GetRolesAsync(User user);

        Task<User> FindByIdAsync(string userId);

        Task<User> FindByNameAsync(string userName);
    }
}
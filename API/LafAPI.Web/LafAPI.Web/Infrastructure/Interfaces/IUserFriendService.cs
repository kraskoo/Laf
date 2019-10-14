namespace LafAPI.Web.Infrastructure.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using LafAPI.Data.Models;
    using LafAPI.Web.Infrastructure.Models;
    using LafAPI.Web.Models.Account;

    public interface IUserFriendService
    {
        Task<UserFriend> GetAsync(string userId, string friendId);

        Task<FriendsViewModel> GetFriendsAsync(
            string userId,
            bool hasPredicates = false,
            params Func<UserFromUserFriendViewModel, bool>[] predicates);

        IQueryable<UserFriend> GetAll(params Expression<Func<UserFriend, bool>>[] predicates);

        Task<List<UserViewModel>> GetByUserIdAndStatus(
            string userId,
            FriendshipStatusType status);

        Task<List<UserViewModel>> GetByFriendIdAndStatus(
            string friendId,
            FriendshipStatusType status);

        Task<int> CreateAsync(string userId, string friendId);

        Task<FriendshipConfirmationResult> ConfirmAsync(string userId, string friendId);

        Task<int> InvitationsCount(string userId);

        Task DropFriendship(string userId, string friendId);

        Task BlockFriendship(string id, string userId);
    }
}
﻿namespace LafAPI.Web.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using LafAPI.Common.Mapping;
    using LafAPI.Data.Common.Repositories;
    using LafAPI.Data.Models;
    using LafAPI.Web.Infrastructure.Interfaces;
    using LafAPI.Web.Infrastructure.Models;
    using LafAPI.Web.Models.Account;

    using Microsoft.EntityFrameworkCore;

    public class UserFriendService : IUserFriendService
    {
        private const string SuccessfullyFriendshipConfirmation = "Successfully accepted new friend!";
        private const string FailedFriendshipConfirmation = "There's no request for friendship!";

        private readonly IRepository<UserFriend> repository;

        public UserFriendService(IRepository<UserFriend> repository) =>
            this.repository = repository;

        public Task<UserFriend> GetAsync(string userId, string friendId) =>
            this.repository.GetByIdAsync(userId, friendId);

        public async Task<FriendsViewModel> GetFriendsAsync(
            string userId,
            params Func<UserFromUserFriendViewModel, bool>[] predicates) =>
            await this.GetFriendsViewModel(userId, predicates);

        public IQueryable<UserFriend> GetAll(params Expression<Func<UserFriend, bool>>[] predicates) =>
            predicates.Aggregate<Expression<Func<UserFriend, bool>>, IQueryable<UserFriend>>(
                null,
                (current, predicate) => current == null ?
                    this.repository.All()
                        .Include(uf => uf.User)
                        .Include(uf => uf.Friend)
                        .Where(predicate) :
                    current.Where(predicate));

        public Task<List<UserViewModel>> GetByUserIdAndStatus(
            string userId,
            FriendshipStatusType status) =>
            this.repository.All()
                .Include(uf => uf.Friend)
                .Where(uf => uf.UserId == userId && uf.Status == status)
                .Select(uf => uf.Friend)
                .To<UserViewModel>()
                .ToListAsync();

        public Task<List<UserViewModel>> GetByFriendIdAndStatus(
            string friendId,
            FriendshipStatusType status) =>
            this.repository.All()
                .Include(uf => uf.User)
                .Where(uf => uf.FriendId == friendId && uf.Status == status)
                .Select(uf => uf.User)
                .To<UserViewModel>()
                .ToListAsync();

        public Task<int> CreateAsync(string userId, string friendId)
        {
            var userFriend = new UserFriend
                                 {
                                     UserId = userId,
                                     FriendId = friendId,
                                     Status = FriendshipStatusType.Awaitable
                                 };
            var friendUser = new UserFriend
                                 {
                                     UserId = friendId,
                                     FriendId = userId,
                                     Status = FriendshipStatusType.Invited
                                 };
            this.repository.Add(userFriend);
            this.repository.Add(friendUser);
            return this.repository.SaveChangesAsync();
        }

        public async Task<FriendshipConfirmationResult> ConfirmAsync(string userId, string friendId)
        {
            var friendshipConfirmation = new FriendshipConfirmationResult
                                            {
                                                Message = SuccessfullyFriendshipConfirmation,
                                                Status = 200
                                            };
            var userFriend = await this.repository.All()
                                 .FirstOrDefaultAsync(
                                     uf => uf.UserId == friendId &&
                                           uf.FriendId == userId &&
                                           uf.Status == FriendshipStatusType.Awaitable);
            var friendUser = await this.repository.All()
                                 .FirstOrDefaultAsync(
                                     uf => uf.UserId == userId &&
                                           uf.FriendId == friendId &&
                                           uf.Status == FriendshipStatusType.Invited);
            if (userFriend != null && friendUser != null)
            {
                userFriend.Status = FriendshipStatusType.Accepted;
                friendUser.Status = FriendshipStatusType.Accepted;
                this.repository.Update(userFriend);
                this.repository.Update(friendUser);
            }
            else
            {
                friendshipConfirmation.Message = FailedFriendshipConfirmation;
                friendshipConfirmation.Status = 400;
            }

            await this.repository.SaveChangesAsync();
            return await Task.FromResult(friendshipConfirmation);
        }

        public async Task<int> InvitationsCount(string userId)
        {
            var list = await this.GetAll(uf => uf.UserId == userId && uf.Status == FriendshipStatusType.Invited)
                            .To<UserFromUserFriendViewModel>()
                            .ToListAsync();
            return this.GetInvitations(list, userId).Count;
        }

        public async Task DropFriendship(string userId, string friendId)
        {
            var userFriend = await this.GetAsync(userId, friendId);
            var friendUser = await this.GetAsync(friendId, userId);
            if (userFriend != null && friendUser != null)
            {
                this.repository.Delete(userFriend);
                this.repository.Delete(friendUser);
                await this.repository.SaveChangesAsync();
            }
        }

        public async Task BlockFriendship(string id, string userId)
        {
            var userFriend = await this.GetAsync(id, userId);
            if (userFriend != null)
            {
                userFriend.Status = FriendshipStatusType.Blocked;
                this.repository.Update(userFriend);
                await this.repository.SaveChangesAsync();
            }
        }

        private List<UserViewModel> Get(
            IEnumerable<UserFromUserFriendViewModel> list,
            Func<UserFromUserFriendViewModel, bool> predicate,
            Func<UserFromUserFriendViewModel, UserViewModel> selectionPredicate,
            Func<UserFromUserFriendViewModel, bool> additionalPredicate = null)
        {
            if (additionalPredicate == null)
            {
                return list.Where(predicate)
                    .Select(selectionPredicate)
                    .ToList();
            }

            return list.Where(predicate)
                .Where(additionalPredicate)
                .Select(selectionPredicate)
                .ToList();
        }

        private List<UserViewModel> GetUserFriends(
            IEnumerable<UserFromUserFriendViewModel> list,
            string userId,
            params Func<UserFromUserFriendViewModel, bool>[] predicates) =>
            this.Get(
                list,
                uf =>
                    uf.Id == userId && uf.Status == FriendshipStatusType.Accepted,
                this.FromFriend,
                predicates.Length > 1 ? predicates[0] : null);

        private List<UserViewModel> GetInvitedFriends(
            IEnumerable<UserFromUserFriendViewModel> list,
            string userId,
            params Func<UserFromUserFriendViewModel, bool>[] predicates) =>
            this.Get(
                list,
                uf =>
                    uf.FriendId == userId &&
                    uf.Status == FriendshipStatusType.Invited,
                this.FromUser,
                predicates.Length > 1 ? predicates[1] : null);

        private List<UserViewModel> GetInvitations(
            IEnumerable<UserFromUserFriendViewModel> list,
            string userId,
            params Func<UserFromUserFriendViewModel, bool>[] predicates) =>
            this.Get(
                list,
                uf =>
                    uf.Id == userId &&
                    uf.Status == FriendshipStatusType.Invited,
                this.FromFriend,
                predicates.Length > 1 ? predicates[0] : null);

        private List<UserViewModel> GetAwaitableFriends(
            IEnumerable<UserFromUserFriendViewModel> list,
            string userId,
            params Func<UserFromUserFriendViewModel, bool>[] predicates) =>
            this.Get(
                list,
                uf =>
                    uf.FriendId == userId &&
                    uf.Status == FriendshipStatusType.Awaitable,
                this.FromUser,
                predicates.Length > 1 ? predicates[1] : null);

        private async Task<FriendsViewModel> GetFriendsViewModel(
            string userId,
            params Func<UserFromUserFriendViewModel, bool>[] predicates)
        {
            var viewModels = await this.GetAll(
                    uf =>
                        (uf.UserId == userId &&
                            (uf.Status == FriendshipStatusType.Accepted || uf.Status == FriendshipStatusType.Awaitable)) ||
                        ((uf.UserId == userId || uf.FriendId == userId) && uf.Status == FriendshipStatusType.Invited))
                .To<UserFromUserFriendViewModel>()
                .ToListAsync();
            return new FriendsViewModel
            {
                Friends = this.GetUserFriends(viewModels, userId, predicates),
                AwaitableFriends = this.GetAwaitableFriends(viewModels, userId, predicates),
                InvitedFriends = this.GetInvitedFriends(viewModels, userId, predicates),
                Invitations = this.GetInvitations(viewModels, userId, predicates)
            };
        }

        private UserViewModel FromUser(UserFromUserFriendViewModel model) =>
            new UserViewModel
                {
                    Id = model.Id,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

        private UserViewModel FromFriend(UserFromUserFriendViewModel model) =>
            new UserViewModel
                {
                    Id = model.FriendId,
                    Email = model.FriendEmail,
                    FirstName = model.FriendFirstName,
                    LastName = model.FriendLastName
                };
    }
}
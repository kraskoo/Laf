namespace LafAPI.Web.Infrastructure.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LafAPI.Data.Models;
    using LafAPI.Web.Infrastructure.Interfaces;
    using LafAPI.Web.Models.Account;
    using LafAPI.Web.Models.Message;

    public class MessageService : IMessageService
    {
        private readonly ConcurrentDictionary<int, MessageViewModel> messages;
        private readonly ConcurrentDictionary<string, User> users;

        public MessageService()
        {
            this.messages = new ConcurrentDictionary<int, MessageViewModel>();
            this.users = new ConcurrentDictionary<string, User>();
        }

        public async Task<IEnumerable<MessageViewModel>> GetAll(string userId, string friendId) =>
            await Task.FromResult(
                this.messages.Values
                    .Where(v => (v.User.Id == userId && v.Friend.Id == friendId) ||
                                (v.Friend.Id == userId && v.User.Id == friendId))
                    .OrderBy(v => v.CreationDate));

        public async Task<bool> Create(
            UserViewModel user,
            UserViewModel friend,
            DateTime creationDate,
            string text)
        {
            var succeed = false;
            var id = this.GetId(user.Id, friend.Id, creationDate);
            if (!this.messages.ContainsKey(id))
            {
                var newMessage = new MessageViewModel
                                     {
                                         CreationDate = creationDate,
                                         User = user,
                                         Friend = friend,
                                         Text = text
                                     };
                succeed = this.messages.TryAdd(id, newMessage);
            }

            return await Task.FromResult(succeed);
        }

        public async Task<bool> Edit(string userId, string friendId, DateTime creationDate, string editedMessage)
        {
            var id = this.GetId(userId, friendId, creationDate);
            var succeed = this.messages.TryGetValue(id, out var viewModel);
            if (succeed)
            {
                var newViewModel = viewModel;
                newViewModel.Text = editedMessage;
                succeed = this.messages.TryUpdate(id, newViewModel, viewModel);
            }

            return await Task.FromResult(succeed);
        }

        public async Task<bool> Delete(string userId, string friendId, DateTime creationDate)
        {
            var id = this.GetId(userId, friendId, creationDate);
            var result = this.messages.TryGetValue(id, out _);
            if (result)
            {
                result = this.messages.TryRemove(id, out _);
            }

            return await Task.FromResult(result);
        }

        public async Task<bool> IsUserExist(string userId) =>
            await Task.FromResult(this.users.ContainsKey(userId));

        public async Task<bool> AddUser(User user) =>
            await Task.FromResult(this.users.TryAdd(user.Id, user));

        public async Task<User> GetUser(string id)
        {
            this.users.TryGetValue(id, out var user);
            return await Task.FromResult(user);
        }

        public UserViewModel ToUserViewMode(User user) =>
            new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

        private int GetId(string userId, string friendId, DateTime creationDate) =>
            $"{userId}-{friendId}({creationDate.Ticks})".GetHashCode();
    }
}
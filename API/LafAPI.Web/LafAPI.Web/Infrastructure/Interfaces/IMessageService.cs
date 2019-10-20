namespace LafAPI.Web.Infrastructure.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LafAPI.Data.Models;
    using LafAPI.Web.Models.Account;
    using LafAPI.Web.Models.Message;

    public interface IMessageService
    {
        Task<IEnumerable<MessageViewModel>> GetAll(string userId, string friendId);

        Task<bool> Create(UserViewModel user, UserViewModel friend, DateTime creationDate, string text);

        Task<bool> Edit(string userId, string friendId, DateTime creationDate, string editedMessage);

        Task<bool> Delete(string userId, string friendId, DateTime creationDate);

        Task<bool> IsUserExist(string userId);

        Task<bool> AddUser(User user);

        Task<User> GetUser(string id);

        UserViewModel ToUserViewMode(User user);
    }
}
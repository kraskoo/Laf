namespace LafAPI.Web.Infrastructure.Interfaces
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using LafAPI.Data.Models;

    public interface IMessageService
    {
        IQueryable<Message> GetAll(string userId = null, string friendId = null);

        Task<int> Create(string userId, string friendId, string message);

        Task<int> Edit(string userId, string friendId, DateTime creationDate, string editedMessage);

        Task<int> Delete(string userId, string friendId, DateTime creationDate);
    }
}
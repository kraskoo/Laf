namespace LafAPI.Web.Infrastructure.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using LafAPI.Data.Common.Repositories;
    using LafAPI.Data.Models;
    using LafAPI.Web.Infrastructure.Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class MessageService : IMessageService
    {
        private readonly IRepository<Message> repository;

        public MessageService(IRepository<Message> repository) =>
            this.repository = repository;

        public IQueryable<Message> GetAll(string userId = null, string friendId = null) =>
            userId == null && friendId == null ?
                this.repository.All() :
                this.repository.All().Where(
                    m => (m.UserId == userId && m.FriendId == friendId) || (m.UserId == friendId && m.FriendId == userId));

        public async Task<int> Create(string userId, string friendId, string message)
        {
            this.repository.Add(new Message
                                    {
                                        UserId = userId,
                                        FriendId = friendId,
                                        CreationDate = DateTime.Now,
                                        Text = message
                                    });
            return await this.repository.SaveChangesAsync();
        }

        public async Task<int> Edit(string userId, string friendId, DateTime creationDate, string editedMessage)
        {
            var msgToEdit = await this.repository.All().FirstOrDefaultAsync(
                m => m.UserId == userId && m.FriendId == friendId && m.CreationDate == creationDate);
            if (msgToEdit != null)
            {
                msgToEdit.Text = editedMessage;
                this.repository.Update(msgToEdit);
            }

            return await this.repository.SaveChangesAsync();
        }

        public async Task<int> Delete(string userId, string friendId, DateTime creationDate)
        {
            var toDelete = await this.repository.GetByIdAsync(userId, friendId, creationDate);
            if (toDelete != null)
            {
                this.repository.Delete(toDelete);
            }

            return await this.repository.SaveChangesAsync();
        }
    }
}
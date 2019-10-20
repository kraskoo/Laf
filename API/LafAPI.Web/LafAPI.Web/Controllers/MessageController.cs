namespace LafAPI.Web.Controllers
{
    using System.Threading.Tasks;

    using LafAPI.Web.Infrastructure.Extensions;
    using LafAPI.Web.Infrastructure.Interfaces;
    using LafAPI.Web.Models.Account;
    using LafAPI.Web.Models.Message;

    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]")]
    public class MessageController : BaseController
    {
        private readonly IMessageService messageService;

        public MessageController(IMessageService messageService, IUserService userService) : base(userService)
            => this.messageService = messageService;

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Messages(FriendBindingModel model)
        {
            var friendId = model.FriendId;
            var result = await this.ValidateFriend(friendId);
            if (result != null)
            {
                return result;
            }

            return this.Ok(await this.messageService.GetAll(this.User.GetId(), friendId));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(MessageBindingModel model)
        {
            var id = this.User.GetId();
            if (string.IsNullOrEmpty(id))
            {
                return this.BadRequest("User doesn't exist!");
            }

            var result = await this.messageService.IsUserExist(id);
            if (!result)
            {
                await this.messageService.AddUser(await this.UserServices.FindByIdAsync(id));
            }

            result = await this.messageService.IsUserExist(model.FriendId);
            if (!result)
            {
                await this.messageService.AddUser(await this.UserServices.FindByIdAsync(model.FriendId));
            }

            var user = await this.messageService.GetUser(id);
            var friend = await this.messageService.GetUser(model.FriendId);
            await this.messageService.Create(
                this.messageService.ToUserViewMode(user),
                this.messageService.ToUserViewMode(friend),
                model.CreationDate,
                model.Text);
            return this.Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Edit(MessageBindingModel model)
        {
            var id = this.User.GetId();
            var result = await this.ValidateFriend(model.FriendId);
            if (result != null)
            {
                return result;
            }

            await this.messageService.Edit(
                id,
                model.FriendId,
                model.CreationDate,
                model.Text);
            return this.Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Delete(MessageBindingModel model)
        {
            var id = this.User.GetId();
            var result = await this.ValidateFriend(model.FriendId);
            if (result != null)
            {
                return result;
            }

            await this.messageService.Delete(id, model.FriendId, model.CreationDate);
            return this.Ok();
        }
    }
}
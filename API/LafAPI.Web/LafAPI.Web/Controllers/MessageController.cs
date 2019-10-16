namespace LafAPI.Web.Controllers
{
    using System.Threading.Tasks;

    using LafAPI.Common.Mapping;
    using LafAPI.Web.Infrastructure.Extensions;
    using LafAPI.Web.Infrastructure.Interfaces;
    using LafAPI.Web.Models.Account;
    using LafAPI.Web.Models.Message;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("[controller]")]
    public class MessageController : BaseController
    {
        private readonly IMessageService messageService;

        public MessageController(IMessageService messageService, IUserService userService) : base(userService)
            => this.messageService = messageService;

        public async Task<IActionResult> Messages(FriendBindingModel model)
        {
            var friendId = model.Id;
            var result = await this.ValidateFriend(friendId);
            if (result != null)
            {
                return result;
            }

            return this.Ok(await this.messageService.GetAll(this.User.GetId(), friendId)
                               .To<MessageViewModel>()
                               .ToListAsync());
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public IActionResult Test()
        {
            return this.Ok("Test");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(MessageBindingModel model)
        {
            var id = this.User.GetId();
            var result = await this.ValidateFriend(model.Id);
            if (result != null)
            {
                return result;
            }

            await this.messageService.Create(id, model.Id, model.Text);
            return this.Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Edit(MessageBindingModel model)
        {
            var id = this.User.GetId();
            var result = await this.ValidateFriend(model.Id);
            if (result != null)
            {
                return result;
            }

            await this.messageService.Edit(id, model.Id, model.CreationDate, model.Text);
            return this.Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Delete(MessageBindingModel model)
        {
            var id = this.User.GetId();
            var result = await this.ValidateFriend(model.Id);
            if (result != null)
            {
                return result;
            }

            await this.messageService.Delete(id, model.Id, model.CreationDate);
            return this.Ok();
        }
    }
}
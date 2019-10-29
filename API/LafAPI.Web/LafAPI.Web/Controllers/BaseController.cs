namespace LafAPI.Web.Controllers
{
    using System.Threading.Tasks;

    using LafAPI.Web.Infrastructure.Interfaces;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class BaseController : ControllerBase
    {
        protected BaseController(IUserService userService) =>
            this.UserServices = userService;

        protected IUserService UserServices { get; }

        protected async Task<IActionResult> ValidateFriend(string friendId)
        {
            if (string.IsNullOrEmpty(friendId))
            {
                this.ModelState.AddModelError("Empty Id", "You must provide User Id!");
                return this.BadRequest(this.ModelState);
            }

            var friend = await this.UserServices.FindByIdAsync(friendId);
            if (friend == null)
            {
                this.ModelState.AddModelError("Non existing user", "You must provide existing User Id!");
                return this.BadRequest(this.ModelState);
            }

            return null;
        }
    }
}
namespace LafAPI.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LafAPI.Common.Mapping;
    using LafAPI.Data.Models;
    using LafAPI.Web.Infrastructure.Extensions;
    using LafAPI.Web.Infrastructure.Interfaces;
    using LafAPI.Web.Infrastructure.Models;
    using LafAPI.Web.Infrastructure.Services;
    using LafAPI.Web.Models.Account;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]")]
    public class AccountController : BaseController
    {
        private readonly IUserFriendService userFriendService;

        public AccountController(
            IUserService userService,
            IUserFriendService userFriendService) : base(userService)
            => this.userFriendService = userFriendService;

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> InvitationsCount() =>
            this.Ok(await this.userFriendService.InvitationsCount(this.User.GetId()));

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Reject([FromBody]FriendBindingModel model)
        {
            var userId = model.Id;
            var result = await this.ValidateFriend(userId);
            if (result != null)
            {
                return result;
            }

            var id = this.User.GetId();
            await this.userFriendService.Reject(id, userId);
            return this.Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> DropFriendship([FromBody]FriendBindingModel model)
        {
            var userId = model.Id;
            var id = this.User.GetId();
            var result = await this.ValidateFriend(userId);
            if (result != null)
            {
                return result;
            }

            await this.userFriendService.DropFriendship(id, userId);
            return this.Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> BlockUser([FromBody]FriendBindingModel model)
        {
            var userId = model.Id;
            var id = this.User.GetId();
            var result = await this.ValidateFriend(userId);
            if (result != null)
            {
                return result;
            }

            await this.userFriendService.BlockUser(id, userId);
            return this.Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UnblockUser([FromBody]FriendBindingModel model)
        {
            var userId = model.Id;
            var result = await this.ValidateFriend(userId);
            if (result != null)
            {
                return result;
            }

            var id = this.User.GetId();
            await this.userFriendService.UnblockUser(id, userId);
            return this.Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        [IgnoreAntiforgeryToken]
        public IActionResult Login([FromBody]UserLoginBindingModel model)
        {
            var response = this.UserServices.Authenticate(
                               model.Email,
                               model.Password,
                               this.HttpContext,
                               out var user);
            if (response.Result == UserService.InvalidEmail)
            {
                this.ModelState.AddModelError(
                    nameof(UserService.InvalidEmail),
                    UserService.InvalidEmail);
                return this.BadRequest(this.ModelState);
            }

            if (response.Result == UserService.InvalidPassword)
            {
                this.ModelState.AddModelError(
                    nameof(UserService.InvalidPassword),
                    UserService.InvalidPassword);
                return this.BadRequest(this.ModelState);
            }

            response.Id = user.Id;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Email = user.Email;
            return this.Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Register([FromBody]UserRegisterBindingModel model)
        {
            if (model == null || !this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            var user = new User
                           {
                               FirstName = model.FirstName,
                               LastName = model.LastName,
                               Email = model.Email,
                               UserName = model.Email
                           };
            var role = !await this.UserServices.AnyAsync() ?
                           nameof(RoleType.Administrator) :
                           nameof(RoleType.Regular);
            var creationResult = await this.UserServices.CreateAsync(user, model.Password);
            if (!creationResult.Succeeded)
            {
                return this.BadRequest(this.ModelState);
            }

            var addToRoleResult = await this.UserServices.AddToRoleAsync(user, role);
            if (!addToRoleResult.Succeeded)
            {
                return this.BadRequest(addToRoleResult.GetAllErrors(this.ModelState));
            }

            var response = this.UserServices.Authenticate(
                               model.Email,
                               model.Password,
                               this.HttpContext,
                               out user);
            if (response.Result == UserService.InvalidEmail)
            {
                this.ModelState.AddModelError(
                    nameof(UserService.InvalidEmail),
                    UserService.InvalidEmail);
                return this.BadRequest(this.ModelState);
            }

            if (response.Result == UserService.InvalidPassword)
            {
                this.ModelState.AddModelError(
                    nameof(UserService.InvalidPassword),
                    UserService.InvalidPassword);
                return this.BadRequest(this.ModelState);
            }

            response.Id = user.Id;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Email = user.Email;
            return this.Ok(response);
        }

        [HttpGet]
        [Route("[action]/{id:guid}")]
        public async Task<IActionResult> ById(string id)
        {
            var user = await this.UserServices.FindByIdAsync(id);
            var roles = await this.UserServices.GetRolesAsync(user);
            var response = new LoginResponse
                               {
                                   Id = user.Id,
                                   FirstName = user.FirstName,
                                   LastName = user.LastName,
                                   Email = user.Email,
                                   Roles = roles
                               };
            return this.Ok(response);
        }

        [HttpGet]
        [Route("inroles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = nameof(RoleType.Administrator))]
        public async Task<IActionResult> AllInRoles([FromQuery]string roles)
        {
            var enumerableRoles = roles.Split(',').Select(r => r.Trim());
            var users = new HashSet<User>();
            foreach (var role in enumerableRoles)
            {
                var usersInRole = await this.UserServices.GetUsersInRoleAsync(role);
                foreach (var user in usersInRole)
                {
                    users.Add(user);
                }
            }

            return this.Ok(new EnumerableQuery<User>(users).To<UserViewModel>());
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Friends([FromQuery]string search = null, [FromQuery]bool friends = false)
        {
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
            }

            var id = this.User.GetId();
            if (friends)
            {
                return this.Ok(
                    !string.IsNullOrEmpty(search) ?
                        await this.userFriendService.GetFriendsAsync(
                            id,
                            uf => uf.FriendFirstName.ToLower().Contains(search) ||
                                  uf.FriendLastName.ToLower().Contains(search) ||
                                  uf.FriendEmail.ToLower().Contains(search),
                            uf => uf.FirstName.ToLower().Contains(search) ||
                                  uf.LastName.ToLower().Contains(search) ||
                                  uf.Email.ToLower().Contains(search)) :
                        await this.userFriendService.GetFriendsAsync(id));
            }

            return this.Ok(
                !string.IsNullOrEmpty(search) ?
                    this.UserServices.UsersWithoutFriendshipWithCurrentUser(
                            id,
                            u => u.FirstName.ToLower().Contains(search) ||
                                 u.LastName.ToLower().Contains(search) ||
                                 u.Email.ToLower().Contains(search))
                        .To<UserViewModel>() :
                    this.UserServices.UsersWithoutFriendshipWithCurrentUser(id)
                        .To<UserViewModel>());
        }

        [HttpGet]
        [Route("uncofirmedfriendships")]
        public async Task<IActionResult> GetUncofirmedFriendships() =>
            this.Ok(
                await this.userFriendService.GetByFriendIdAndStatus(
                        this.User.GetId(),
                        FriendshipStatusType.Invited));

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddFriend([FromBody]FriendBindingModel model)
        {
            var id = this.User.GetId();
            var friendId = model.Id;
            var validationResult = await this.ValidateFriend(friendId);
            if (validationResult != null)
            {
                return validationResult;
            }

            var userFriendship = await this.userFriendService.GetAsync(id, friendId);
            if (userFriendship != null)
            {
                this.ModelState.AddModelError("Existing friendship", "You cannot add same friendship twice!");
                return this.BadRequest(this.ModelState);
            }

            await this.userFriendService.CreateAsync(id, friendId);
            return this.Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ConfirmFriendship([FromBody]FriendBindingModel model)
        {
            var id = this.User.GetId();
            var friendId = model.Id;
            var validationResult = await this.ValidateFriend(friendId);
            if (validationResult != null)
            {
                return validationResult;
            }

            var result = await this.userFriendService.ConfirmAsync(id, friendId);
            if (result.Status != 200)
            {
                return this.BadRequest(result);
            }

            return this.Ok();
        }
    }
}
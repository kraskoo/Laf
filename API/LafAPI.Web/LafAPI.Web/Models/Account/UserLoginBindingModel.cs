namespace LafAPI.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    using LafAPI.Common.Mapping;
    using LafAPI.Data.Models;

    public class UserLoginBindingModel : IMapFrom<User>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
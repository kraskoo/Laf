namespace LafAPI.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    using LafAPI.Common;
    using LafAPI.Common.Mapping;
    using LafAPI.Data.Models;

    public class UserRegisterBindingModel : IMapFrom<User>
    {
        [Required]
        [MinLength(GlobalConstants.MinNameLength)]
        [MaxLength(GlobalConstants.MaxNameLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(GlobalConstants.MinNameLength)]
        [MaxLength(GlobalConstants.MaxNameLength)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string PasswordConfirmation { get; set; }
    }
}
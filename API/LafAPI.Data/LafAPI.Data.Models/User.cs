namespace LafAPI.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using LafAPI.Common;
    using LafAPI.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public sealed class User : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.UserFriends = new HashSet<UserFriend>();
            this.AvatarPath = "/profile-picture.png";
        }

        [Required]
        [MinLength(GlobalConstants.MinNameLength)]
        [MaxLength(GlobalConstants.MaxNameLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(GlobalConstants.MinNameLength)]
        [MaxLength(GlobalConstants.MaxNameLength)]
        public string LastName { get; set; }

        public string AvatarPath { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<IdentityUserRole<string>> Roles { get; set; }

        public ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public ICollection<UserFriend> UserFriends { get; set; }
    }
}
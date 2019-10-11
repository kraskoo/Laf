namespace LafAPI.Data.Models
{
    using System;

    using LafAPI.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public sealed class Role : IdentityRole, IAuditInfo, IDeletableEntity
    {
        public Role() : this(null)
        {
        }

        public Role(string name) : base(name)
            => this.Id = Guid.NewGuid().ToString();

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
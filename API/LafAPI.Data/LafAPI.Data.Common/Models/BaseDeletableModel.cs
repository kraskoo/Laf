namespace LafAPI.Data.Common.Models
{
    using System;

    public abstract class BaseDeletableModel<TKey> : BaseModel<TKey>, IDeletableEntity where TKey : IEquatable<TKey>
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
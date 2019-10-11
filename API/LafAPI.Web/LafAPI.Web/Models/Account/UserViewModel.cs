namespace LafAPI.Web.Models.Account
{
    using LafAPI.Common.Mapping;
    using LafAPI.Data.Models;

    public class UserViewModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public override int GetHashCode() =>
            this.Id.GetHashCode();
    }
}
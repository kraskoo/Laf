namespace LafAPI.Data.Repositories
{
    using LafAPI.Data.Models;

    public class UserFriendRepository : EfRepository<UserFriend>
    {
        public UserFriendRepository(LafContext context) : base(context)
        {
        }
    }
}
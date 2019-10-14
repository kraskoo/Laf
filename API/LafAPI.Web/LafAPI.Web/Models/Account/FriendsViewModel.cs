namespace LafAPI.Web.Models.Account
{
    using System.Collections.Generic;

    public class FriendsViewModel
    {
        public ICollection<UserViewModel> Friends { get; set; }

        public ICollection<UserViewModel> InvitedUsers { get; set; }

        public ICollection<UserViewModel> Invitations { get; set; }

        public ICollection<UserViewModel> BlockedUsers { get; set; }
    }
}
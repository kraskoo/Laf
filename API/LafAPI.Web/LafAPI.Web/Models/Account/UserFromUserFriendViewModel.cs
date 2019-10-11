namespace LafAPI.Web.Models.Account
{
    using AutoMapper;

    using LafAPI.Common.Mapping;
    using LafAPI.Data.Models;

    public class UserFromUserFriendViewModel : IMapFrom<UserFriend>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string FriendId { get; set; }

        public string FriendFirstName { get; set; }

        public string FriendLastName { get; set; }

        public string FriendEmail { get; set; }

        public FriendshipStatusType Status { get; set; }

        public override int GetHashCode() =>
            this.Email.GetHashCode();

        public void CreateMappings(IProfileExpression configuration) =>
            configuration.CreateMap<UserFriend, UserFromUserFriendViewModel>()
                .ForMember(m => m.Status, opt => opt.MapFrom(x => x.Status))
                .ForMember(m => m.Id, opt => opt.MapFrom(x => x.User.Id))
                .ForMember(m => m.Email, opt => opt.MapFrom(x => x.User.Email))
                .ForMember(m => m.FirstName, opt => opt.MapFrom(x => x.User.FirstName))
                .ForMember(m => m.LastName, opt => opt.MapFrom(x => x.User.LastName))
                .ForMember(m => m.FriendId, opt => opt.MapFrom(x => x.Friend.Id))
                .ForMember(m => m.FriendEmail, opt => opt.MapFrom(x => x.Friend.Email))
                .ForMember(m => m.FriendFirstName, opt => opt.MapFrom(x => x.Friend.FirstName))
                .ForMember(m => m.FriendLastName, opt => opt.MapFrom(x => x.Friend.LastName));
    }
}
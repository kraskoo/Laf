namespace LafAPI.Web.Models.Message
{
    using System;

    using AutoMapper;

    using LafAPI.Common.Mapping;
    using LafAPI.Data.Models;
    using LafAPI.Web.Models.Account;

    public class MessageViewModel : IMapFrom<Message>, IHaveCustomMappings
    {
        public int Id =>
            $"{this.User.Id}-{this.Friend.Id}({this.CreationDate.Ticks}): {this.Text}".GetHashCode();

        public UserViewModel User { get; set; }

        public UserViewModel Friend { get; set; }

        public DateTime CreationDate { get; set; }

        public string Text { get; set; }

        public void CreateMappings(IProfileExpression configuration) =>
            configuration.CreateMap<Message, MessageViewModel>()
                .ForMember(m => m.User, opt => opt.MapFrom(x => x.User))
                .ForMember(m => m.Friend, opt => opt.MapFrom(x => x.Friend));

        public override int GetHashCode() => this.Id;
    }
}
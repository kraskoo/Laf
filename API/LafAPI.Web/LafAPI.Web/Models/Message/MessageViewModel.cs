﻿namespace LafAPI.Web.Models.Message
{
    using System;

    using AutoMapper;

    using LafAPI.Common.Mapping;
    using LafAPI.Data.Models;
    using LafAPI.Web.Models.Account;

    public class MessageViewModel : IMapFrom<Message>, IHaveCustomMappings
    {
        public string UserId { get; set; }

        public UserViewModel User { get; set; }

        public string FriendId { get; set; }

        public UserViewModel Friend { get; set; }

        public DateTime CreationDate { get; set; }

        public string Text { get; set; }

        public void CreateMappings(IProfileExpression configuration) =>
            configuration.CreateMap<Message, MessageViewModel>()
                .ForMember(m => m.User, opt => opt.MapFrom(x => x.User))
                .ForMember(m => m.Friend, opt => opt.MapFrom(x => x.Friend));
    }
}
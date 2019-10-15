namespace LafAPI.Data.Models
{
    using System;

    public class Message
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public string FriendId { get; set; }

        public User Friend { get; set; }

        public DateTime CreationDate { get; set; }

        public string Text { get; set; }
    }
}
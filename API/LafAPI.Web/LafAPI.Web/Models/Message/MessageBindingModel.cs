namespace LafAPI.Web.Models.Message
{
    using System;

    public class MessageBindingModel
    {
        public string FriendId { get; set; }

        public DateTime CreationDate { get; set; }

        public string Text { get; set; }
    }
}
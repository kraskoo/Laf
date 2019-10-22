namespace LafAPI.Web.Models.Message
{
    using System;

    using LafAPI.Web.Models.Account;

    public class MessageViewModel
    {
        public int Id =>
            $"{this.User.Id}-{this.Friend.Id}({this.CreationDate.Ticks}): {this.Text}".GetHashCode();

        public UserViewModel User { get; set; }

        public UserViewModel Friend { get; set; }

        public DateTime CreationDate { get; set; }

        public string Text { get; set; }

        public override int GetHashCode() => this.Id;
    }
}
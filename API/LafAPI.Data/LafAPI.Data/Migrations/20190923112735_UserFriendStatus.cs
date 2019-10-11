namespace LafAPI.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UserFriendStatus : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Status", "UserFriends");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>("Status", "UserFriends", nullable: false, defaultValue: 0);
        }
    }
}
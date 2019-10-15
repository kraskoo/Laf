namespace LafAPI.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Messages : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Messages");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Messages",
                table => new
                             {
                                 UserId = table.Column<string>(),
                                 FriendId = table.Column<string>(),
                                 CreationDate = table.Column<DateTime>(),
                                 Text = table.Column<string>(nullable: true)
                             },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_Messages", x => new { x.UserId, x.FriendId, x.CreationDate });
                        table.ForeignKey(
                            "FK_Messages_AspNetUsers_FriendId",
                            x => x.FriendId,
                            "AspNetUsers",
                            "Id",
                            onDelete: ReferentialAction.Restrict);
                        table.ForeignKey(
                            "FK_Messages_AspNetUsers_UserId",
                            x => x.UserId,
                            "AspNetUsers",
                            "Id",
                            onDelete: ReferentialAction.Restrict);
                    });

            migrationBuilder.CreateIndex("IX_Messages_FriendId", "Messages", "FriendId");
        }
    }
}
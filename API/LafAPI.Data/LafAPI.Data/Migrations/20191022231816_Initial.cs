namespace LafAPI.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Initial : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("AspNetRoleClaims");
            migrationBuilder.DropTable("AspNetUserClaims");
            migrationBuilder.DropTable("AspNetUserLogins");
            migrationBuilder.DropTable("AspNetUserRoles");
            migrationBuilder.DropTable("AspNetUserTokens");
            migrationBuilder.DropTable("UserFriends");
            migrationBuilder.DropTable("AspNetRoles");
            migrationBuilder.DropTable("AspNetUsers");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "AspNetRoles",
                table => new
                             {
                                 Id = table.Column<string>(),
                                 Name = table.Column<string>(maxLength: 256, nullable: true),
                                 NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                                 ConcurrencyStamp = table.Column<string>(nullable: true),
                                 CreatedOn = table.Column<DateTime>(),
                                 ModifiedOn = table.Column<DateTime>(nullable: true),
                                 IsDeleted = table.Column<bool>(),
                                 DeletedOn = table.Column<DateTime>(nullable: true)
                             },
                constraints: table => { table.PrimaryKey("PK_AspNetRoles", x => x.Id); });
            migrationBuilder.CreateTable(
                "AspNetUsers",
                table => new
                             {
                                 Id = table.Column<string>(),
                                 UserName = table.Column<string>(maxLength: 256, nullable: true),
                                 NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                                 Email = table.Column<string>(maxLength: 256, nullable: true),
                                 NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                                 EmailConfirmed = table.Column<bool>(),
                                 PasswordHash = table.Column<string>(nullable: true),
                                 SecurityStamp = table.Column<string>(nullable: true),
                                 ConcurrencyStamp = table.Column<string>(nullable: true),
                                 PhoneNumber = table.Column<string>(nullable: true),
                                 PhoneNumberConfirmed = table.Column<bool>(),
                                 TwoFactorEnabled = table.Column<bool>(),
                                 LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                                 LockoutEnabled = table.Column<bool>(),
                                 AccessFailedCount = table.Column<int>(),
                                 FirstName = table.Column<string>(maxLength: 35),
                                 LastName = table.Column<string>(maxLength: 35),
                                 AvatarPath = table.Column<string>(nullable: true),
                                 CreatedOn = table.Column<DateTime>(),
                                 ModifiedOn = table.Column<DateTime>(nullable: true),
                                 IsDeleted = table.Column<bool>(),
                                 DeletedOn = table.Column<DateTime>(nullable: true)
                             },
                constraints: table => { table.PrimaryKey("PK_AspNetUsers", x => x.Id); });
            migrationBuilder.CreateTable(
                "AspNetRoleClaims",
                table => new
                             {
                                 Id = table.Column<int>().Annotation("SqlServer:Identity", "1, 1"),
                                 RoleId = table.Column<string>(),
                                 ClaimType = table.Column<string>(nullable: true),
                                 ClaimValue = table.Column<string>(nullable: true)
                             },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                        table.ForeignKey(
                            "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                            x => x.RoleId,
                            "AspNetRoles",
                            "Id",
                            onDelete: ReferentialAction.Restrict);
                    });
            migrationBuilder.CreateTable(
                "AspNetUserClaims",
                table => new
                             {
                                 Id = table.Column<int>().Annotation("SqlServer:Identity", "1, 1"),
                                 UserId = table.Column<string>(),
                                 ClaimType = table.Column<string>(nullable: true),
                                 ClaimValue = table.Column<string>(nullable: true)
                             },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                        table.ForeignKey(
                            "FK_AspNetUserClaims_AspNetUsers_UserId",
                            x => x.UserId,
                            "AspNetUsers",
                            "Id",
                            onDelete: ReferentialAction.Restrict);
                    });
            migrationBuilder.CreateTable(
                "AspNetUserLogins",
                table => new
                             {
                                 LoginProvider = table.Column<string>(),
                                 ProviderKey = table.Column<string>(),
                                 ProviderDisplayName = table.Column<string>(nullable: true),
                                 UserId = table.Column<string>()
                             },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                        table.ForeignKey(
                            "FK_AspNetUserLogins_AspNetUsers_UserId",
                            x => x.UserId,
                            "AspNetUsers",
                            "Id",
                            onDelete: ReferentialAction.Restrict);
                    });
            migrationBuilder.CreateTable(
                "AspNetUserRoles",
                table => new { UserId = table.Column<string>(), RoleId = table.Column<string>() },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                        table.ForeignKey(
                            "FK_AspNetUserRoles_AspNetRoles_RoleId",
                            x => x.RoleId,
                            "AspNetRoles",
                            "Id",
                            onDelete: ReferentialAction.Restrict);
                        table.ForeignKey(
                            "FK_AspNetUserRoles_AspNetUsers_UserId",
                            x => x.UserId,
                            "AspNetUsers",
                            "Id",
                            onDelete: ReferentialAction.Restrict);
                    });
            migrationBuilder.CreateTable(
                "AspNetUserTokens",
                table => new
                             {
                                 UserId = table.Column<string>(),
                                 LoginProvider = table.Column<string>(),
                                 Name = table.Column<string>(),
                                 Value = table.Column<string>(nullable: true)
                             },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                        table.ForeignKey(
                            "FK_AspNetUserTokens_AspNetUsers_UserId",
                            x => x.UserId,
                            "AspNetUsers",
                            "Id",
                            onDelete: ReferentialAction.Restrict);
                    });
            migrationBuilder.CreateTable(
                "UserFriends",
                table => new
                             {
                                 UserId = table.Column<string>(),
                                 FriendId = table.Column<string>(),
                                 Status = table.Column<int>()
                             },
                constraints: table =>
                    {
                        table.PrimaryKey("PK_UserFriends", x => new { x.UserId, x.FriendId });
                        table.ForeignKey(
                            "FK_UserFriends_AspNetUsers_FriendId",
                            x => x.FriendId,
                            "AspNetUsers",
                            "Id",
                            onDelete: ReferentialAction.Restrict);
                        table.ForeignKey(
                            "FK_UserFriends_AspNetUsers_UserId",
                            x => x.UserId,
                            "AspNetUsers",
                            "Id",
                            onDelete: ReferentialAction.Restrict);
                    });
            migrationBuilder.CreateIndex("IX_AspNetRoleClaims_RoleId", "AspNetRoleClaims", "RoleId");
            migrationBuilder.CreateIndex("IX_AspNetRoles_IsDeleted", "AspNetRoles", "IsDeleted");
            migrationBuilder.CreateIndex(
                "RoleNameIndex",
                "AspNetRoles",
                "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");
            migrationBuilder.CreateIndex("IX_AspNetUserClaims_UserId", "AspNetUserClaims", "UserId");
            migrationBuilder.CreateIndex("IX_AspNetUserLogins_UserId", "AspNetUserLogins", "UserId");
            migrationBuilder.CreateIndex("IX_AspNetUserRoles_RoleId", "AspNetUserRoles", "RoleId");
            migrationBuilder.CreateIndex("IX_AspNetUsers_IsDeleted", "AspNetUsers", "IsDeleted");
            migrationBuilder.CreateIndex("EmailIndex", "AspNetUsers", "NormalizedEmail");
            migrationBuilder.CreateIndex(
                "UserNameIndex",
                "AspNetUsers",
                "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
            migrationBuilder.CreateIndex("IX_UserFriends_FriendId", "UserFriends", "FriendId");
        }
    }
}
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FRTools.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscordLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ChannelId = table.Column<long>(type: "bigint", nullable: false),
                    Module = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Command = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordServers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServerId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordServers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DragonCache",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DragonType = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    BodyGene = table.Column<int>(type: "int", nullable: false),
                    BodyColor = table.Column<int>(type: "int", nullable: false),
                    WingGene = table.Column<int>(type: "int", nullable: false),
                    WingColor = table.Column<int>(type: "int", nullable: false),
                    TertiaryGene = table.Column<int>(type: "int", nullable: false),
                    TertiaryColor = table.Column<int>(type: "int", nullable: false),
                    EyeType = table.Column<int>(type: "int", nullable: false),
                    Element = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    SHA1Hash = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    FRDragonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DragonCache", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FRDominances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    First = table.Column<int>(type: "int", nullable: false),
                    Second = table.Column<int>(type: "int", nullable: false),
                    Third = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRDominances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FRUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FRId = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelatedEntity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Heartbeat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Errors = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PinglistCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PinglistCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfileSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileBio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicProfile = table.Column<bool>(type: "bit", nullable: false),
                    ShowPreviewsOnProfile = table.Column<bool>(type: "bit", nullable: false),
                    ShowSkinsOnProfile = table.Column<bool>(type: "bit", nullable: false),
                    ShowPingListsOnProfile = table.Column<bool>(type: "bit", nullable: false),
                    DefaultShowSkinsInBrowse = table.Column<bool>(type: "bit", nullable: false),
                    DefaultSkinsArePublic = table.Column<bool>(type: "bit", nullable: false),
                    ShowFRLinkStatus = table.Column<bool>(type: "bit", nullable: false),
                    ShowAds = table.Column<bool>(type: "bit", nullable: false),
                    DefaultAdvancedCoverageBackgroundColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultAdvancedCoverageOverlayColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultAdvancedCoverageDummyOpacity = table.Column<int>(type: "int", nullable: false),
                    DefaultAdvancedCoverageSkinOpacity = table.Column<int>(type: "int", nullable: false),
                    DefaultAdvancedCoveragePercentagePrecision = table.Column<int>(type: "int", nullable: false),
                    DefaultAdvancedCoverageUseDressingRoomBase = table.Column<bool>(type: "bit", nullable: false),
                    DefaultAdvancedCoverageScry = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FRTopicId = table.Column<int>(type: "int", nullable: false),
                    FRTopicName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopicStarter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopicStarterClanId = table.Column<int>(type: "int", nullable: false),
                    FRClaimedReplyCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Server_Id = table.Column<int>(type: "int", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordSettings_DiscordServers_Server_Id",
                        column: x => x.Server_Id,
                        principalTable: "DiscordServers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FRItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FRId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemCategory = table.Column<int>(type: "int", nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rarity = table.Column<int>(type: "int", nullable: true),
                    AssetUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TreasureValue = table.Column<int>(type: "int", nullable: true),
                    ItemType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FoodType = table.Column<int>(type: "int", nullable: true),
                    FoodValue = table.Column<int>(type: "int", nullable: true),
                    RequiredLevel = table.Column<int>(type: "int", nullable: true),
                    Creator_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FRItems_FRUsers_Creator_Id",
                        column: x => x.Creator_Id,
                        principalTable: "FRUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileSettings_Id = table.Column<int>(type: "int", nullable: true),
                    FRUser_Id = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_FRUsers_FRUser_Id",
                        column: x => x.FRUser_Id,
                        principalTable: "FRUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_ProfileSettings_ProfileSettings_Id",
                        column: x => x.ProfileSettings_Id,
                        principalTable: "ProfileSettings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role_Id = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_Roles_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Topic_Id = table.Column<int>(type: "int", nullable: true),
                    FRPostId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostAuthor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostAuthorClanId = table.Column<int>(type: "int", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reports = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Topics_Topic_Id",
                        column: x => x.Topic_Id,
                        principalTable: "Topics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FRItemFlashSales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Item_Id = table.Column<int>(type: "int", nullable: true),
                    DiscoveredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RemovedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FRItemFlashSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FRItemFlashSales_FRItems_Item_Id",
                        column: x => x.Item_Id,
                        principalTable: "FRItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.User_Id, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pinglists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneratedId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecretKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Creator_Id = table.Column<int>(type: "int", nullable: true),
                    PinglistCategory_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pinglists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pinglists_PinglistCategories_PinglistCategory_Id",
                        column: x => x.PinglistCategory_Id,
                        principalTable: "PinglistCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pinglists_Users_Creator_Id",
                        column: x => x.Creator_Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Skins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DragonType = table.Column<int>(type: "int", nullable: false),
                    GenderType = table.Column<int>(type: "int", nullable: false),
                    GeneratedId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecretKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Coverage = table.Column<double>(type: "float", nullable: true),
                    Visibility = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Creator_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skins_Users_Creator_Id",
                        column: x => x.Creator_Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    User_Id = table.Column<int>(type: "int", nullable: false),
                    Role_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.User_Id, x.Role_Id });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_Role_Id",
                        column: x => x.Role_Id,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PingListEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FRUser_Id = table.Column<int>(type: "int", nullable: false),
                    GeneratedId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecretKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pinglist_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PingListEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PingListEntries_FRUsers_FRUser_Id",
                        column: x => x.FRUser_Id,
                        principalTable: "FRUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PingListEntries_Pinglists_Pinglist_Id",
                        column: x => x.Pinglist_Id,
                        principalTable: "Pinglists",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Previews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Skin_Id = table.Column<int>(type: "int", nullable: true),
                    DragonId = table.Column<int>(type: "int", nullable: true),
                    ScryerUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviewImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DragonData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviewTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Requestor_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Previews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Previews_Skins_Skin_Id",
                        column: x => x.Skin_Id,
                        principalTable: "Skins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Previews_Users_Requestor_Id",
                        column: x => x.Requestor_Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_Role_Id",
                table: "AspNetRoleClaims",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordSettings_Server_Id",
                table: "DiscordSettings",
                column: "Server_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DragonCache_SHA1Hash",
                table: "DragonCache",
                column: "SHA1Hash");

            migrationBuilder.CreateIndex(
                name: "IX_FRItemFlashSales_Item_Id",
                table: "FRItemFlashSales",
                column: "Item_Id");

            migrationBuilder.CreateIndex(
                name: "IX_FRItems_Creator_Id",
                table: "FRItems",
                column: "Creator_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PingListEntries_FRUser_Id",
                table: "PingListEntries",
                column: "FRUser_Id");

            migrationBuilder.CreateIndex(
                name: "IX_PingListEntries_Pinglist_Id",
                table: "PingListEntries",
                column: "Pinglist_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Pinglists_Creator_Id",
                table: "Pinglists",
                column: "Creator_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Pinglists_PinglistCategory_Id",
                table: "Pinglists",
                column: "PinglistCategory_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_FRPostId",
                table: "Posts",
                column: "FRPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Topic_Id",
                table: "Posts",
                column: "Topic_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Previews_Requestor_Id",
                table: "Previews",
                column: "Requestor_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Previews_Skin_Id",
                table: "Previews",
                column: "Skin_Id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Skins_Creator_Id",
                table: "Skins",
                column: "Creator_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_FRTopicId",
                table: "Topics",
                column: "FRTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_User_Id",
                table: "UserClaims",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_User_Id",
                table: "UserLogins",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_Role_Id",
                table: "UserRoles",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FRUser_Id",
                table: "Users",
                column: "FRUser_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfileSettings_Id",
                table: "Users",
                column: "ProfileSettings_Id");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DiscordLogs");

            migrationBuilder.DropTable(
                name: "DiscordSettings");

            migrationBuilder.DropTable(
                name: "DragonCache");

            migrationBuilder.DropTable(
                name: "FRDominances");

            migrationBuilder.DropTable(
                name: "FRItemFlashSales");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "PingListEntries");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Previews");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "DiscordServers");

            migrationBuilder.DropTable(
                name: "FRItems");

            migrationBuilder.DropTable(
                name: "Pinglists");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Skins");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "PinglistCategories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "FRUsers");

            migrationBuilder.DropTable(
                name: "ProfileSettings");
        }
    }
}

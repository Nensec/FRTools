﻿// <auto-generated />
using System;
using FRTools.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FRTools.Core.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250401231813_Bundle items")]
    partial class Bundleitems
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FRTools.Core.Data.DataModels.AccountModels.ProfileSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DefaultAdvancedCoverageBackgroundColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DefaultAdvancedCoverageDummyOpacity")
                        .HasColumnType("int");

                    b.Property<string>("DefaultAdvancedCoverageOverlayColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DefaultAdvancedCoveragePercentagePrecision")
                        .HasColumnType("int");

                    b.Property<string>("DefaultAdvancedCoverageScry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DefaultAdvancedCoverageSkinOpacity")
                        .HasColumnType("int");

                    b.Property<bool>("DefaultAdvancedCoverageUseDressingRoomBase")
                        .HasColumnType("bit");

                    b.Property<bool>("DefaultShowSkinsInBrowse")
                        .HasColumnType("bit");

                    b.Property<bool>("DefaultSkinsArePublic")
                        .HasColumnType("bit");

                    b.Property<string>("ProfileBio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PublicProfile")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowAds")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowFRLinkStatus")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowPingListsOnProfile")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowPreviewsOnProfile")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowSkinsOnProfile")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("ProfileSettings");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.AccountModels.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.AccountModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<int?>("FRUserId")
                        .HasColumnType("int")
                        .HasColumnName("FRUser_Id");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<int?>("ProfileSettingsId")
                        .HasColumnType("int")
                        .HasColumnName("ProfileSettings_Id");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("FRUserId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("ProfileSettingsId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.AccountModels.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("User_Id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.AccountModels.UserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("User_Id");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.AccountModels.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("User_Id");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("Role_Id");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.DiscordModels.DiscordLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<long>("ChannelId")
                        .HasColumnType("bigint");

                    b.Property<string>("Command")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Module")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("DiscordLogs");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.DiscordModels.DiscordServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("IconBase64")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ServerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("DiscordServers");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.DiscordModels.DiscordSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ServerId")
                        .HasColumnType("int")
                        .HasColumnName("Server_Id");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.ToTable("DiscordSettings");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.FlightRisingModels.DragonCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int>("BodyColor")
                        .HasColumnType("int");

                    b.Property<int>("BodyGene")
                        .HasColumnType("int");

                    b.Property<int>("DragonType")
                        .HasColumnType("int");

                    b.Property<int>("Element")
                        .HasColumnType("int");

                    b.Property<int>("EyeType")
                        .HasColumnType("int");

                    b.Property<int?>("FRDragonId")
                        .HasColumnType("int");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("SHA1Hash")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<int>("TertiaryColor")
                        .HasColumnType("int");

                    b.Property<int>("TertiaryGene")
                        .HasColumnType("int");

                    b.Property<int>("WingColor")
                        .HasColumnType("int");

                    b.Property<int>("WingGene")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SHA1Hash");

                    b.ToTable("DragonCache");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.FlightRisingModels.FRDominance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("First")
                        .HasColumnType("int");

                    b.Property<int>("Second")
                        .HasColumnType("int");

                    b.Property<int>("Third")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("FRDominances");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.FlightRisingModels.FRItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AssetUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int")
                        .HasColumnName("Creator_Id");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FRId")
                        .HasColumnType("int");

                    b.Property<int?>("FRItemId")
                        .HasColumnType("int")
                        .HasColumnName("FRItem_Id");

                    b.Property<int?>("FoodType")
                        .HasColumnType("int");

                    b.Property<int?>("FoodValue")
                        .HasColumnType("int");

                    b.Property<string>("IconUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ItemCategory")
                        .HasColumnType("int");

                    b.Property<string>("ItemType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Rarity")
                        .HasColumnType("int");

                    b.Property<int?>("RequiredLevel")
                        .HasColumnType("int");

                    b.Property<int?>("TreasureValue")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("FRItemId");

                    b.ToTable("FRItems");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.FlightRisingModels.FRItemFlashSale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DiscoveredAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ItemId")
                        .HasColumnType("int")
                        .HasColumnName("Item_Id");

                    b.Property<DateTime?>("RemovedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("FRItemFlashSales");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.FlightRisingModels.FRUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FRId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FRUsers");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.Job", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Errors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Heartbeat")
                        .HasColumnType("datetime2");

                    b.Property<string>("RelatedEntity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.NewsReaderModels.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<int>("FRPostId")
                        .HasColumnType("int");

                    b.Property<string>("PostAuthor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostAuthorClanId")
                        .HasColumnType("int");

                    b.Property<int>("Reports")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<int?>("TopicId")
                        .HasColumnType("int")
                        .HasColumnName("Topic_Id");

                    b.HasKey("Id");

                    b.HasIndex("FRPostId");

                    b.HasIndex("TopicId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.NewsReaderModels.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FRClaimedReplyCount")
                        .HasColumnType("int");

                    b.Property<int>("FRTopicId")
                        .HasColumnType("int");

                    b.Property<string>("FRTopicName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TopicStarter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TopicStarterClanId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FRTopicId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.PinglistModels.PingListEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FRUserId")
                        .HasColumnType("int")
                        .HasColumnName("FRUser_Id");

                    b.Property<string>("GeneratedId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PinglistId")
                        .HasColumnType("int")
                        .HasColumnName("Pinglist_Id");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecretKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FRUserId");

                    b.HasIndex("PinglistId");

                    b.ToTable("PingListEntries");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.PinglistModels.Pinglist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int")
                        .HasColumnName("Creator_Id");

                    b.Property<string>("Format")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GeneratedId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PinglistCategoryId")
                        .HasColumnType("int")
                        .HasColumnName("PinglistCategory_Id");

                    b.Property<string>("SecretKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("PinglistCategoryId");

                    b.ToTable("Pinglists");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.PinglistModels.PinglistCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PinglistCategories");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.Preview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DragonData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DragonId")
                        .HasColumnType("int");

                    b.Property<string>("PreviewImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PreviewTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RequestorId")
                        .HasColumnType("int")
                        .HasColumnName("Requestor_Id");

                    b.Property<string>("ScryerUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SkinId")
                        .HasColumnType("int")
                        .HasColumnName("Skin_Id");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RequestorId");

                    b.HasIndex("SkinId");

                    b.ToTable("Previews");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.Skin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Coverage")
                        .HasColumnType("float");

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int")
                        .HasColumnName("Creator_Id");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DragonType")
                        .HasColumnType("int");

                    b.Property<int>("GenderType")
                        .HasColumnType("int");

                    b.Property<string>("GeneratedId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecretKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.Property<int>("Visibility")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Skins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("Role_Id");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("User_Id");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.AccountModels.User", b =>
                {
                    b.HasOne("FRTools.Core.Data.DataModels.FlightRisingModels.FRUser", "FRUser")
                        .WithMany()
                        .HasForeignKey("FRUserId");

                    b.HasOne("FRTools.Core.Data.DataModels.AccountModels.ProfileSettings", "ProfileSettings")
                        .WithMany()
                        .HasForeignKey("ProfileSettingsId");

                    b.Navigation("FRUser");

                    b.Navigation("ProfileSettings");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.AccountModels.UserClaim", b =>
                {
                    b.HasOne("FRTools.Core.Data.DataModels.AccountModels.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.AccountModels.UserLogin", b =>
                {
                    b.HasOne("FRTools.Core.Data.DataModels.AccountModels.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.AccountModels.UserRole", b =>
                {
                    b.HasOne("FRTools.Core.Data.DataModels.AccountModels.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FRTools.Core.Data.DataModels.AccountModels.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.DiscordModels.DiscordSetting", b =>
                {
                    b.HasOne("FRTools.Core.Data.DataModels.DiscordModels.DiscordServer", "Server")
                        .WithMany()
                        .HasForeignKey("ServerId");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.FlightRisingModels.FRItem", b =>
                {
                    b.HasOne("FRTools.Core.Data.DataModels.FlightRisingModels.FRUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");

                    b.HasOne("FRTools.Core.Data.DataModels.FlightRisingModels.FRItem", null)
                        .WithMany("BundleItems")
                        .HasForeignKey("FRItemId");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.FlightRisingModels.FRItemFlashSale", b =>
                {
                    b.HasOne("FRTools.Core.Data.DataModels.FlightRisingModels.FRItem", "Item")
                        .WithMany("FlashSales")
                        .HasForeignKey("ItemId");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.NewsReaderModels.Post", b =>
                {
                    b.HasOne("FRTools.Core.Data.DataModels.NewsReaderModels.Topic", "Topic")
                        .WithMany("Posts")
                        .HasForeignKey("TopicId");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.PinglistModels.PingListEntry", b =>
                {
                    b.HasOne("FRTools.Core.Data.DataModels.FlightRisingModels.FRUser", "FRUser")
                        .WithMany()
                        .HasForeignKey("FRUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FRTools.Core.Data.DataModels.PinglistModels.Pinglist", null)
                        .WithMany("Entries")
                        .HasForeignKey("PinglistId");

                    b.Navigation("FRUser");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.PinglistModels.Pinglist", b =>
                {
                    b.HasOne("FRTools.Core.Data.DataModels.AccountModels.User", "Creator")
                        .WithMany("Pinglists")
                        .HasForeignKey("CreatorId");

                    b.HasOne("FRTools.Core.Data.DataModels.PinglistModels.PinglistCategory", "PinglistCategory")
                        .WithMany("Pinglists")
                        .HasForeignKey("PinglistCategoryId");

                    b.Navigation("Creator");

                    b.Navigation("PinglistCategory");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.Preview", b =>
                {
                    b.HasOne("FRTools.Core.Data.DataModels.AccountModels.User", "Requestor")
                        .WithMany("Previews")
                        .HasForeignKey("RequestorId");

                    b.HasOne("FRTools.Core.Data.DataModels.Skin", "Skin")
                        .WithMany("Previews")
                        .HasForeignKey("SkinId");

                    b.Navigation("Requestor");

                    b.Navigation("Skin");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.Skin", b =>
                {
                    b.HasOne("FRTools.Core.Data.DataModels.AccountModels.User", "Creator")
                        .WithMany("Skins")
                        .HasForeignKey("CreatorId");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("FRTools.Core.Data.DataModels.AccountModels.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("FRTools.Core.Data.DataModels.AccountModels.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.AccountModels.User", b =>
                {
                    b.Navigation("Pinglists");

                    b.Navigation("Previews");

                    b.Navigation("Skins");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.FlightRisingModels.FRItem", b =>
                {
                    b.Navigation("BundleItems");

                    b.Navigation("FlashSales");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.NewsReaderModels.Topic", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.PinglistModels.Pinglist", b =>
                {
                    b.Navigation("Entries");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.PinglistModels.PinglistCategory", b =>
                {
                    b.Navigation("Pinglists");
                });

            modelBuilder.Entity("FRTools.Core.Data.DataModels.Skin", b =>
                {
                    b.Navigation("Previews");
                });
#pragma warning restore 612, 618
        }
    }
}

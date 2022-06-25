﻿// <auto-generated />
using System;
using Discord.Bot.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Discord.Bot.Database.Migrations
{
    [DbContext(typeof(UserSayingsContext))]
    [Migration("20220625065146_lazy_loading")]
    partial class lazy_loading
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.6");

            modelBuilder.Entity("Discord.Bot.Database.Models.Saying", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("IsmRecorder")
                        .HasColumnType("TEXT");

                    b.Property<string>("IsmSaying")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserIsmKey")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IsmSaying")
                        .IsUnique();

                    b.HasIndex("UserIsmKey");

                    b.ToTable("Sayings");
                });

            modelBuilder.Entity("Discord.Bot.Database.User", b =>
                {
                    b.Property<string>("IsmKey")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.HasKey("IsmKey");

                    b.HasIndex("GuildId", "IsmKey")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Discord.Bot.Database.Models.Saying", b =>
                {
                    b.HasOne("Discord.Bot.Database.User", null)
                        .WithMany("Sayings")
                        .HasForeignKey("UserIsmKey");
                });

            modelBuilder.Entity("Discord.Bot.Database.User", b =>
                {
                    b.Navigation("Sayings");
                });
#pragma warning restore 612, 618
        }
    }
}

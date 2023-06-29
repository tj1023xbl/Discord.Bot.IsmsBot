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
    [Migration("20230629140016_uniqueconstraint")]
    partial class uniqueconstraint
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

                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IsmKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("IsmRecorder")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("IsmSaying")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IsmKey", "GuildId");

                    b.HasIndex("IsmSaying", "GuildId")
                        .IsUnique();

                    b.ToTable("Sayings");
                });
#pragma warning restore 612, 618
        }
    }
}

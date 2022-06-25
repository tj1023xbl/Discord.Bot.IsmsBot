using System;
using Discord.Bot.Database.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Discord.Bot.Database
{
    public partial class UserSayingsContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Saying> Sayings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "userSayings.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            optionsBuilder.UseSqlite(connection);
        }
    }
}

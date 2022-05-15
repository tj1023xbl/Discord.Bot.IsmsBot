using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Discord.Bot.Database
{
    public partial class UserSayingsContext : DbContext
    {
        public virtual DbSet<UserSayings> UserSayings { get; set; }
        public virtual DbSet<DiscordServer> DiscordServers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "userSayings.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            optionsBuilder.UseSqlite(connection);
        }
    }
}

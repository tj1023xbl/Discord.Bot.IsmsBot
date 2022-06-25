using System;
using System.IO;
using Discord.Bot.Database.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;

namespace Discord.Bot.Database
{
    public class UserSayingsContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Saying> Sayings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {

            string datasource = "userSayings.db";

            // set the absolute DB path to use
            var dbFolderPath = Path.Join(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "IsmsBot");

            if (!Directory.Exists(dbFolderPath))
            {
                Directory.CreateDirectory(dbFolderPath);
            }

            string dbFilePath = Path.Join(dbFolderPath, datasource);

            Log.Information("Database is located at {0}", dbFilePath);

            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = dbFilePath };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            optionsBuilder.UseSqlite(connection);
        }

    }
}

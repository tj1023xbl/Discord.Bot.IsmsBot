using System;
using System.IO;
using Discord.Bot.Database.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;

namespace Discord.Bot.Database
{
    public class AppDBContext : DbContext
    {
        public virtual DbSet<Saying> Sayings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            Log.Debug("Configuring database...");
            string datasource = "userSayings.db";

            // set the absolute DB path to use
            var specialFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
            if(string.IsNullOrEmpty(specialFolder))
            {
                string msg = $"The special folder 'Environment.SpecialFolder.ApplicationData' at '{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}' was not found and was not created.";
                Log.Error(msg);
                throw new ApplicationException(msg);
            }
            var dbFolderPath = Path.Join(
                specialFolder,
                "IsmsBot");

            if (!Directory.Exists(dbFolderPath))
            {
                Log.Debug("Creating DB Directory '{0}'", dbFolderPath);
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

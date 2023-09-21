using Serilog;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Discord.WebSocket;
using Discord.Commands;
using IsmsBot.RegexCommand;
using Discord.Bot.Database;
using Microsoft.Extensions.Hosting;
using Discord.Bot.IsmsBot.Services;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Discord.Bot.IsmsBot
{
    class Program
    {
        private readonly IConfiguration _config;

        public Program()
        {
            // Create the configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");

            // Build the configuration and assign to the config.
            _config = builder.Build();
        }

        public static Task Main(string[] args) => new Program().MainAsync(args);

        /// <summary>
        /// Main Method
        /// </summary>
        /// <returns></returns>
        public async Task MainAsync(string[] args)
        {

            IHostBuilder builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) => ConfigureServices(context, services));
                SetupLogging();

            IHost host = builder.Build();
            await host.RunAsync();
            
        }

        private ServiceProvider ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton(_config)

                // Add regex command services
                .AddScoped<RegexCommandHandler>()
                .AddScoped<CommandOptions>()
                .AddScoped<IRegexCommandModuleProvider, RegexCommandModuleProvider>()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<IDiscordProxy, DiscordProxy>()
                .AddSingleton(typeof(IsmsService))
                .AddDbContext<UserSayingsContext>()
                .AddHostedService<Worker>();


            var serviceProvider = services.BuildServiceProvider();
            using (var dbContext = serviceProvider.GetRequiredService<UserSayingsContext>())
            {
                Log.Verbose("Ensuring the database is created...");
                dbContext.Database.EnsureCreated();
                Log.Information("SQLite database created at '{0}'", dbContext.Database.GetConnectionString());
            }

            return serviceProvider;
        }

        /// <summary>
        /// Sets up logging options.
        /// </summary>
        private void SetupLogging()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "IsmsBot", "IsmsBotLog.log");
            Log.Logger = Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(path, rollingInterval: RollingInterval.Month, rollOnFileSizeLimit: true, fileSizeLimitBytes: 1024*1024*10)
            .CreateLogger();

            Log.Information("Logs will be stored at {0}", path);

        }
    }
}

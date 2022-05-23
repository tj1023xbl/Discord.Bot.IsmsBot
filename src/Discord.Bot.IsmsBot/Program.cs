using Serilog;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Bot.Database;

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

        public static Task Main(string[] args) => new Program().MainAsync();

        /// <summary>
        /// Main Method
        /// </summary>
        /// <returns></returns>
        public async Task MainAsync() 
        {
            using (var services = ConfigureServices())
            {
                SetupLogging();
                var a = services.GetService<IDiscordProxy>();

                try
                {
                    await a.RunDiscordApp();
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }
        }

        private ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection().AddSingleton(_config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<IDiscordProxy, DiscordProxy>()
                .AddSingleton<IIsmsService, IsmsService>()
                .AddDbContext<UserSayingsContext>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        /// <summary>
        /// Sets up logging options.
        /// </summary>
        private void SetupLogging() 
        {
            Log.Logger = Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .WriteTo.Console().WriteTo.File("C:\\logging\\discordBot\\logs.log").CreateLogger();
        }
    }
}

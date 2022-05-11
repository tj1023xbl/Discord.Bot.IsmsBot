using Serilog;
using System;
using System.Threading.Tasks;

namespace Discord.Bot.IsmsBot
{
    class Startup
    {
        public static Task Main(string[] args) => new Startup().MainAsync();

        /// <summary>
        /// Main Method
        /// </summary>
        /// <returns></returns>
        public async Task MainAsync() 
        {
            SetupLogging();
            DiscordProxy discordProxy = new DiscordProxy();

            try
            {
                await discordProxy.RunDiscordApp();
            }
            catch (Exception e) 
            {
                Log.Error(e.Message);
            }
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

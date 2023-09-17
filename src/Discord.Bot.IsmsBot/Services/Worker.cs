using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Discord.Bot.IsmsBot.Services
{
    internal class Worker : BackgroundService
    {
        private readonly IDiscordProxy _discordProxy;

        public Worker(IDiscordProxy discordProxy)
        {
            _discordProxy = discordProxy;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;

        public override async Task StartAsync(CancellationToken cancelToken)
        {
            Log.Information("Starting the Isms Bot...");
            try
            {
                await _discordProxy.RunDiscordApp();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

        }

        public override Task StopAsync(CancellationToken token)
        {
            Log.Information("Shutting down the Isms Bot...");
            return Task.CompletedTask;
        }

    }
}

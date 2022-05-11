using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace Discord.Bot.IsmsBot
{
    public class DiscordProxy
    {
        DiscordSocketClient _disClient;
        public DiscordProxy() 
        {
            _disClient = new DiscordSocketClient();
        }


        public async Task RunDiscordApp() 
        {
            _disClient.Log += DiscordLogAsync;

            Console.WriteLine("Please enter Discord token key");
            var token = Environment.GetEnvironmentVariable(Console.ReadLine(), EnvironmentVariableTarget.Machine);

            if (string.IsNullOrWhiteSpace(token)) 
            {
                throw new Exception("Path did not return a token.");
            }

            await _disClient.LoginAsync(TokenType.Bot, token);
            await _disClient.StartAsync();

            await Task.Delay(-1);
        }

        public static async Task DiscordLogAsync(LogMessage message) 
        {
            var severity = message.Severity switch
            {
                LogSeverity.Critical => LogEventLevel.Fatal,
                LogSeverity.Error => LogEventLevel.Error,
                LogSeverity.Warning => LogEventLevel.Warning,
                LogSeverity.Info => LogEventLevel.Information,
                LogSeverity.Verbose => LogEventLevel.Verbose,
                LogSeverity.Debug => LogEventLevel.Debug,
                _ => LogEventLevel.Information
            };
            Log.Write(severity, message.Exception, "[{Source}] {Message}", message.Source, message.Message);
            await Task.CompletedTask;
        }
    }
}

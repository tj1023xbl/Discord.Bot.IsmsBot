using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;

namespace Discord.Bot.IsmsBot
{
    public class DiscordProxy : IDiscordProxy
    {
        DiscordSocketClient _disClient;
        IConfiguration _configuration;
        CommandHandler _handler;

        /// <summary>
        /// Constructor
        /// </summary>
        public DiscordProxy(IConfiguration conifg, DiscordSocketClient client, CommandHandler handler) 
        {
            _configuration = conifg;
            _disClient = client;
            _handler = handler; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task RunDiscordApp() 
        {
            _disClient.Log += DiscordLogAsync;

            _disClient.LoggedIn += () => {
                Log.Information("Discord bot logged in as {0}", _disClient.CurrentUser);
                return Task.CompletedTask;
            };

            await _disClient.LoginAsync(TokenType.Bot, GetToken());
            await _disClient.StartAsync();
            await _handler.InstallCommandsAsync();

            // Idle until any key is pressed. Then gracefully close the app.
            await Task.Factory.StartNew(() => Console.ReadLine());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private string GetToken() 
        {
            string tokenVar = _configuration.GetSection("TokenVar").Value;
            if (string.IsNullOrWhiteSpace(tokenVar)) 
            {
                Console.WriteLine("Please enter the environment variable key to retrieve the Discord token key");
                tokenVar = Console.ReadLine();
            }
            
            var token = Environment.GetEnvironmentVariable(tokenVar, EnvironmentVariableTarget.Machine);

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception("Environment variable was invalid or did not result in a valid discord token");
            }

            return token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private static Task ClientOnMessageReceived(SocketMessage arg) 
        {
            Log.Debug(arg.Content);
            string response = $"User '{arg.Author.Username}' successfully ran hellowrold!";
            if (arg.Content.StartsWith("!helloworld")) 
            {
                arg.Channel.SendMessageAsync(response);
            }

            return Task.CompletedTask;
        }
    }
}

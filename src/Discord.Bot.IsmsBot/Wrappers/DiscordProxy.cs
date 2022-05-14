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

        /// <summary>
        /// Constructor
        /// </summary>
        public DiscordProxy() 
        {
            _disClient = new DiscordSocketClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task RunDiscordApp() 
        {
            _disClient.Log += DiscordLogAsync;
            _disClient.MessageReceived += ClientOnMessageReceived;


            string token = GetToken();

            await _disClient.LoginAsync(TokenType.Bot, token);
            await _disClient.StartAsync();

            await Task.Delay(-1);
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
            Console.WriteLine("Please enter Discord token key");
            var token = Environment.GetEnvironmentVariable(Console.ReadLine(), EnvironmentVariableTarget.Machine);

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception("Path did not return a token.");
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

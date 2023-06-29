using Discord.Commands;
using Discord.WebSocket;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Bot.IsmsBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly CommandService _commands;
        private readonly IServiceProvider _servicecs;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="commands"></param>
        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            _discordClient = client;
            _commands = commands;
            _servicecs = services;
        }

        public async Task InstallCommandsAsync()
        {
            _discordClient.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _servicecs);

            _commands.CommandExecuted += async (optional, context, result) =>
            {
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    // the command failed, let's notify the user that something happened.
                    await context.Channel.SendMessageAsync($"error: {result}");
                    Log.ForContext("Result", result, true)
                    .Error("An error occurred while executing `{0}`. Result: {1}", context.Message, result);
                }
            };

            foreach (var module in _commands.Modules)
            {
                Log.Verbose("CommandHandler Module '{0}' initialized.", module.Name);
            }
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_discordClient.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            Log.Verbose("Creating message context");

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_discordClient, message);

            Log.Verbose("Context created successfully: {0}", context);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _servicecs);
        }
    }
}

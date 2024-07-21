using Discord.Commands;
using Discord.Interactions;
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
        private readonly InteractionService _interactions;
        private readonly IServiceProvider _services;

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="commands"></param>
        /// <param name="interactions"></param>
        /// <param name="services"></param>
        public CommandHandler(DiscordSocketClient client, CommandService commands, InteractionService interactions, IServiceProvider services)
        {
            _discordClient = client;
            _commands = commands;
            _interactions = interactions;
            _services = services;
        }

        public async Task InstallCommandsAsync()
        {
            _discordClient.MessageReceived += HandleCommandAsync;
            _discordClient.InteractionCreated += HandleInteractionAsync;

            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _services);
            await _interactions.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _services);
            await _interactions.RegisterCommandsGloballyAsync();

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

        private async Task HandleInteractionAsync(SocketInteraction interaction) 
        {
            var context = new SocketInteractionContext(_discordClient, interaction);
            await _interactions.ExecuteCommandAsync(context, _services);
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
                services: _services);
        }
    }
}

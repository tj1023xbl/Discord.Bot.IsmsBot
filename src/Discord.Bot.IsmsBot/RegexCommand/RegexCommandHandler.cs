using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using IsmsBot.RegexCommand;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace IsmsBot.RegexCommand
{
    public class RegexCommandHandler : IDisposable
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandOptions _commandOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _log;
        private ICollection<RegexCommandInstance> _commands;
        private readonly SemaphoreSlim _lock;
        private CancellationToken _hostCancellationToken;

        public RegexCommandHandler(IServiceProvider serviceProvider, DiscordSocketClient client, CommandOptions commandOptions)
        {
            this._client = client;
            this._commandOptions = commandOptions;
            this._serviceProvider = serviceProvider;
            this._log = Log.Logger;
            this._commands = new List<RegexCommandInstance>();
            this._lock = new SemaphoreSlim(1, 1);
            this._client.MessageReceived += HandleCommandAsync;
        }

        public async Task InitializeCommandsAsync()
        {
            try
            {
                await this._lock.WaitAsync(_hostCancellationToken);
                this._log.Debug("Initializing commands");

                this._commands.Clear();
                CommandOptions options = this._commandOptions;
                foreach (Assembly asm in options.Assemblies)
                {
                    Log.Verbose("Adding assembly '{0}'", asm.FullName);
                    this.AddAssembly(asm);
                    Log.Verbose("Finished adding assembly '{0}'", asm.FullName);

                }
                foreach (Type t in options.Classes)
                {
                    Log.Verbose("Adding type from options.Classes '{0}'", t.Name);
                    this.AddType(t.GetTypeInfo());
                    Log.Verbose("Finished adding type from options.Classes '{0}'", t.Name);
                }

                this._commands = _commands.OrderByDescending(cmd => cmd.Priority).ToArray();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "There was an error in the initialization method 'InitializeCommandsAsync'");
                throw;
            }
            finally
            {
                this._lock.Release();
            }
        }

        private void AddAssembly(Assembly assembly)
        {
            IEnumerable<TypeInfo> types = assembly.DefinedTypes.Where(t => !t.IsAbstract && !t.ContainsGenericParameters
                && !Attribute.IsDefined(t, typeof(CompilerGeneratedAttribute)) && Attribute.IsDefined(t, typeof(LoadRegexCommandsAttribute)));
            if (!types.Any())
            {
                _log.Warning("Cannot initialize Regex commands from assembly {AssemblyName} - no non-static non-abstract classes with {Attribute}", assembly.FullName, nameof(LoadRegexCommandsAttribute));
                return;
            }
            foreach (TypeInfo type in types)
            {
                Log.Verbose("Adding type '{0}'", type.Name);
                AddType(type);
                Log.Verbose("Finished adding method '{0}'", type.Name);

            }
        }

        private void AddType(TypeInfo type)
        {
            IEnumerable<MethodInfo> methods = type.DeclaredMethods.Where(m => !m.IsStatic && !Attribute.IsDefined(m, typeof(CompilerGeneratedAttribute)) && Attribute.IsDefined(m, typeof(RegexCommandAttribute)));
            if (!methods.Any())
            {
                _log.Warning("Cannot initialize Regex command from type {TypeName} - no method with {Attribute}", type.FullName, nameof(RegexCommandAttribute));
                return;
            }
            foreach (MethodInfo method in methods)
            {
                Log.Verbose("Adding method '{0}'", method.Name);
                AddMethod(method);
                Log.Verbose("Finished adding method '{0}'", method.Name);
            }
        }

        private void AddMethod(MethodInfo method)
        {
            IEnumerable<RegexCommandAttribute> attributes = method.GetCustomAttributes<RegexCommandAttribute>();
            if (!attributes.Any())
            {
                _log.Warning("Cannot initialize Regex command from {TypeName}'s method {MethodName} - {Attribute} missing", method.DeclaringType.FullName, method.Name, nameof(RegexCommandAttribute));
                return;
            }
            foreach (RegexCommandAttribute attribute in attributes)
            {
                Log.Verbose("Adding command '{0}'", nameof(attribute));
                _commands.Add(RegexCommandInstance.Build(method, attribute, _serviceProvider));
                Log.Verbose("Finished adding command '{0}'", nameof(attribute));
            }
        }

        private async Task HandleCommandAsync(SocketMessage msg)
        {
            // most of the implementation here taken from https://discord.foxbot.me/docs/guides/commands/intro.html
            // with my own pinch of customizations - TehGM

            // Don't process the command if it was a system message
            if (!(msg is SocketUserMessage message))
                return;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            CommandOptions options = this._commandOptions;
            // only execute if not a bot message
            if (!options.AcceptBotMessages && message.Author.IsBot)
                return;
            // get prefix and argPos
            int argPos = 0;
            bool requirePrefix = msg.Channel is SocketGuildChannel ? options.RequirePublicMessagePrefix : options.RequirePrivateMessagePrefix;
            bool hasStringPrefix = message.HasStringPrefix(options.Prefix, ref argPos);
            bool hasMentionPrefix = false;
            if (!hasStringPrefix)
                hasMentionPrefix = message.HasMentionPrefix(_client.CurrentUser, ref argPos);

            // if prefix not found but is required, return
            if (requirePrefix && (!string.IsNullOrWhiteSpace(options.Prefix) && !hasStringPrefix) && (options.AcceptMentionPrefix && !hasMentionPrefix))
                return;

            // Create a WebSocket-based command context based on the message
            SocketCommandContext context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.

            // Keep in mind that result does not indicate a return value
            // rather an object stating if the command executed successfully.
            await _lock.WaitAsync(_hostCancellationToken).ConfigureAwait(false);
            try
            {
                foreach (RegexCommandInstance command in _commands)
                {
                    _log.Verbose("Executing command {0} with context {1} in module {2}", command.MethodName, context, command.ModuleType);
                    try
                    {
                        IResult preconditionsResult = await command.CheckPreconditionsAsync(context, _serviceProvider);
                        if (!preconditionsResult.IsSuccess)
                            continue;
                        ExecuteResult result = (ExecuteResult)await command.ExecuteAsync(
                            context: context,
                            argPos: argPos,
                            services: _serviceProvider,
                            cancellationToken: _hostCancellationToken)
                            .ConfigureAwait(false);
                        if (result.IsSuccess)
                            return;
                    }
                    catch (OperationCanceledException) { return; }
                    catch (Exception ex)
                    {
                        _log.Error(ex, "Unhandled Exception when executing command {0}", command.MethodName);
                        return;
                    }
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public void Dispose()
        {
            this._client.MessageReceived -= HandleCommandAsync;
            this._lock.Dispose();
        }
    }
}

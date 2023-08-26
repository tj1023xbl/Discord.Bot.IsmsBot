using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Bot.Database.Models;
using IsmsBot.RegexCommand;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;
using System.Text.RegularExpressions;

namespace Discord.Bot.IsmsBot
{
    [LoadRegexCommands]
    public class IsmsModule : ModuleBase<SocketCommandContext>
    {
        private readonly IsmsService _ismsService;
        

        /// <summary>
        /// Constructor
        /// </summary>
        public IsmsModule(IsmsService ismsService)
        {
            _ismsService = ismsService;
        }

        [Command("add")]
        [Summary("Adds an ism message")]
        public async Task AddIsmAsync([Remainder][Summary("The ism to create")] string ismText)
        {
            var user = await _ismsService.AddIsmAsync(ismText, Context);
            if (user != null)
            {
                await Context.Channel.SendMessageAsync($"Successfully added new saying for {user.IsmKey}");
            }
            else
            {
                await Context.Channel.SendMessageAsync(ErrorResponses.AddIsmsError);
            }
        }

        [RegexCommand(@"^(?<key>[a-zA-Z]+ism) (?<command>list)$")]
        public async Task listIsmAsync(Match match)
        {
            var ism = match.Groups.GetValueOrDefault("key").Value.ToLower();
            var sayings = await _ismsService.GetAllIsmsAsync(ism, Context);

            if (sayings != null && sayings.Any())
            {
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendLine($"Sayings for {ism}:");
                int count = 0;
                foreach (Saying saying in sayings)
                {
                    count++;

                    string msg = $"{saying.IsmSaying} | added by {saying.IsmRecorder} on {saying.DateCreated}";

                    // Make sure the output is less than 2000 characters
                    if ((msg.Length + stringBuilder.Length) >= 2000)
                    {
                        await Context.Channel.SendMessageAsync(stringBuilder.ToString());
                        stringBuilder.Clear();
                    }

                    stringBuilder.AppendLine(msg);
                }

                await Context.Channel.SendMessageAsync(stringBuilder.ToString());
            } else
            {
                await Context.Channel.SendMessageAsync($"{ism} has no sayings on this server yet.");
            }
        }

        [Command("random")]
        public async Task GetRandomIsmAsync()
        {
            Saying saying = await _ismsService.GetRandomSayingAsync(Context);

            if(saying == null)
            {
                await Context.Channel.SendMessageAsync($"Couldn't find any isms to display");
            }

            await Context.Channel.SendMessageAsync($"{saying.IsmSaying} - {saying.IsmKey.Replace("ism", "")} | Added by {saying.IsmRecorder} on {saying.DateCreated}");

        }

        [RegexCommand(@"^[a-zA-Z]+ism$")]
        public async Task GetIsmAsync()
        {
            string username = Context.Message.Content.Substring(1).ToLower();
            Log.Verbose("Message content = {0}", username);
            if (string.IsNullOrEmpty(username)) return;

            Saying ism = await _ismsService.GetIsmAsync(username, Context);
            if (ism == null)
            {
                string msg = $"{username} is not recognized, or they don't have any isms on this server yet.";
                Log.Information(msg);
                await Context.Channel.SendMessageAsync(msg);
                return;
            }

            await Context.Channel.SendMessageAsync($"'_{ism.IsmSaying}_' - {username.Replace("ism", "")}");

        }

    }
}

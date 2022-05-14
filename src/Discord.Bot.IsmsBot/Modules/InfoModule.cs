using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace Discord.Bot.IsmsBot.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string echo) => ReplyAsync(echo);
    }

    //public class IsmModule : ModuleBase<SocketCommandContext> 
    //{
    //    [Command("")]
    //}
}

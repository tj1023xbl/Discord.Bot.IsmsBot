using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Discord.Bot.IsmsBot
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        public async Task SayAsync([Remainder][Summary("The text to echo")] string echo) => await ReplyAsync(echo);

	   // ~sample square 20 -> 400
	   [Command("square")]
	   [Summary("Squares a number.")]
	   public async Task SquareAsync(
		   [Summary("The number to square.")]
		int num)
	   {
		  // We can also access the channel from the Command Context.
		  await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num, 2)}");
	   }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Discord.Bot.IsmsBot
{
    [Group("isms")]
    public class IsmsModule : ModuleBase<SocketCommandContext>
    {
	   private readonly IIsmsService _ismsService;

	   /// <summary>
	   /// Constructor
	   /// </summary>
	   public IsmsModule(IIsmsService ismsService) 
	   {
		  _ismsService = ismsService;  
	   }

	   [Command("add")]
	   [Summary("Adds an ism message")]
	   public async Task AddIsmAsync([Remainder][Summary("The ism to create")] string ismText) 
	   {
		  var sayings = await _ismsService.AddIsmAsync(ismText, Context);
		  if (sayings != null)
		  {
			 await Context.Channel.SendMessageAsync($"Successfully added new {sayings.IsmKey}");
		  }
		  else 
		  {
			 await Context.Channel.SendMessageAsync(ErrorResponses.AddIsmsError);
		  }
	   }

	   // ~sample square 20 -> 400
	   [Command("square")]
	   [Summary("Squares a number.")]
	   public async Task SquareAsync( [Summary("The number to square.")] int num)
	   {
		  // We can also access the channel from the Command Context.
		  await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num, 2)}");
	   }
    }
}

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
			 await Context.Channel.SendMessageAsync($"Successfully added new saying for {sayings.Username}");
		  }
		  else 
		  {
			 await Context.Channel.SendMessageAsync(ErrorResponses.AddIsmsError);
		  }
	   }

    }
}

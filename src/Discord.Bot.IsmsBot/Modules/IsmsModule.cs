using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Bot.Database.Models;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;

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
		  var user = await _ismsService.AddIsmAsync(ismText, Context);
		  if (user != null)
		  {
			 await Context.Channel.SendMessageAsync($"Successfully added new saying for {user.Username}");
		  }
		  else 
		  {
			 await Context.Channel.SendMessageAsync(ErrorResponses.AddIsmsError);
		  }
	   }

		[Command("tylerism")]
		public async Task GetIsmAsync(string username)
        {
			if (string.IsNullOrEmpty(username)) return;

			Saying ism = await _ismsService.GetIsmAsync(username, Context);
            if (ism == null)
            {
				string msg = $"{username} is not recognized, or does not have any isms yet.";
				Log.Information(msg);
				await Context.Message.ReplyAsync(msg);
			}

			await Context.Message.ReplyAsync($"_{ism.IsmSaying}_");

		}

	}
}

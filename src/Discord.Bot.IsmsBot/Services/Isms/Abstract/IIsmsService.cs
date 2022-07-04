using Discord.Bot.Database;
using Discord.Bot.Database.Models;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Bot.IsmsBot
{
    public interface IIsmsService
    {
        public Task<User> AddIsmAsync(string commandString, SocketCommandContext context);
        Task<Saying> GetIsmAsync(string username, SocketCommandContext discordContext);
        Task<List<Saying>> GetAllIsmsAsync(string ism);
    }
}

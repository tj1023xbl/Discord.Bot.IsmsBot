using Discord.Bot.Database;
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
        public Task<UserSayings> AddIsmAsync(string commandString, SocketCommandContext context);
    }
}

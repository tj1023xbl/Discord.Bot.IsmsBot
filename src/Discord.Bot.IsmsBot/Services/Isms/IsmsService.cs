using Discord.Bot.Database;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Discord.Bot.IsmsBot
{
    public class IsmsService : IIsmsService
    {
        private readonly UserSayingsContext _dbContext;

        private const string ismPattern = "(?<ismKey>[\\s\\S]+ism)\\s\"(?<ism>[\\s\\S]+)\"";

        public IsmsService(UserSayingsContext dbContext) 
        {
            _dbContext = dbContext;
        }


        public async Task<UserSayings> AddIsmAsync(string commandString, SocketCommandContext context)
        {
            UserSayings userSayingsContext = null;
            if (!string.IsNullOrWhiteSpace(commandString)) 
            {
                
                await ValidateDiscordServer(context);

                var match = Regex.Match(commandString, ismPattern);
                string key = match.Groups["ismKey"].Value;
                string ism = match.Groups["ism"].Value;

                if (!match.Success) 
                {
                    return null;
                }

                userSayingsContext = _dbContext.UserSayings.AsQueryable().Where(u => u.Username == key).FirstOrDefault();

                if (userSayingsContext == null)
                {
                    userSayingsContext = new UserSayings()
                    {
                        GuildId = context.Guild.Id,
                        Username = key,
                        Sayings = new List<string>() { ism }
                    };

                    _dbContext.UserSayings.Add(userSayingsContext);
                }
                else 
                {
                    userSayingsContext.Sayings.Add(ism);
                }

                await _dbContext.SaveChangesAsync();
            }

            return userSayingsContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task ValidateDiscordServer(SocketCommandContext context) 
        {
            List<DiscordServer> servers = _dbContext.DiscordServers.AsQueryable().Where(s => s.Id == context.Guild.Id).ToList();
            if (!servers.Any()) 
            {
                _dbContext.DiscordServers.Add(new DiscordServer()
                {
                    Id = context.Guild.Id,
                    ServerName = context.Guild.Name,
                });

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

using Discord.Bot.Database;
using Discord.Bot.Database.Models;
using Discord.Commands;
using Serilog;
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

        public IsmsService(
            UserSayingsContext dbContext
            ) 
        {
            _dbContext = dbContext;
        }


        public async Task<User> AddIsmAsync(string commandString, SocketCommandContext discordContext)
        {
            User userContext = null;
            if (!string.IsNullOrWhiteSpace(commandString)) 
            {
                // match the regex pattern for the command string `userism "phrase"`
                var match = Regex.Match(commandString, ismPattern);

                if (!match.Success) 
                {
                    return null;
                }

                string username = match.Groups["ismKey"].Value;
                string ism = match.Groups["ism"].Value;

                userContext = await _dbContext.Users.FindAsync(username);

                if (userContext == null)
                {
                    userContext = new User()
                    {
                        GuildId = discordContext.Guild.Id,
                        Username = username,
                        Sayings = new List<Saying>() { 
                            new Saying()
                            {
                                Username = username,
                                DateCreated = DateTime.Now,
                                IsmRecorder = discordContext.User.Username,
                                IsmSaying = ism
                            }
                        }
                    };

                    _dbContext.Users.Add(userContext);
                }
                else 
                {
                    userContext.Sayings.Add(
                        new Saying()
                        {
                            Username=username,
                            DateCreated=DateTime.Now,
                            IsmRecorder=discordContext.User.Username,
                            IsmSaying = ism
                        });
                }

                await _dbContext.SaveChangesAsync();
            }

            return userContext;
        }

        public async Task<Saying> GetIsmAsync(string username, SocketCommandContext discordContext)
        {
            User user = await _dbContext.Users.FindAsync(username);
            if(user == null)
            {
                Log.Information("{0} is not recognized as a user", username);
                return null;
            }

            // Get random saying
            var rand = new Random();
            int toSkip = 0;
            int count = user.Sayings.Count;
            if (count > 0) {
                toSkip = rand.Next(1, count);
            }
            Saying saying = user.Sayings.Skip(toSkip).FirstOrDefault();

            if(saying == null)
            {
                Log.Information("{0} does not have any isms yet.", username);
                return null;
            }

            return saying;
        }

    }
}

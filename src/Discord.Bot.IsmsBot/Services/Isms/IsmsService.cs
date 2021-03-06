using Discord.Bot.Database;
using Discord.Bot.Database.Models;
using Discord.Commands;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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
            Log.Verbose("Adding saying for `{0}`", commandString);
            User userContext = null;
            if (!string.IsNullOrWhiteSpace(commandString))
            {
                // match the regex pattern for the command string `userism "phrase"`
                var match = Regex.Match(commandString, ismPattern);

                if (!match.Success)
                {
                    return null;
                }

                string ismKey = match.Groups["ismKey"].Value.ToLower();
                string ism = match.Groups["ism"].Value;
                try
                {
                    // Try to get the userism from the database
                    userContext = await _dbContext.Users.FindAsync(ismKey);
                    if (userContext != null)
                    {
                        // load the sayings for this userism
                        await _dbContext.Entry(userContext).Collection(u => u.Sayings).LoadAsync();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Error getting user {0}", ex);
                    throw;
                }

                // If the user doesn't exist, create the new userism
                if (userContext == null)
                {
                    userContext = new User()
                    {
                        GuildId = discordContext.Guild.Id,
                        IsmKey = ismKey,
                        Sayings = new List<Saying>() {
                            new Saying()
                            {
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
                            DateCreated = DateTime.Now,
                            IsmRecorder = discordContext.User.Username,
                            IsmSaying = ism
                        });
                }

                await _dbContext.SaveChangesAsync();
            }

            return userContext;
        }

        public async Task<Saying> GetIsmAsync(string ismKey, SocketCommandContext discordContext)
        {
            ismKey = ismKey.ToLower();

            User user = await _dbContext
                .Users
                .FindAsync(ismKey);

            if (user == null)
            {
                Log.Information("{0} is not recognized as a user", ismKey);
                return null;
            }

            // load the sayings for this user
            await _dbContext.Entry(user).Collection(u => u.Sayings).LoadAsync();

            if (user.Sayings == null || !user.Sayings.Any())
            {
                Log.Information("{0} does not have any isms yet.", ismKey);
                return null;
            }

            // Get random saying
            int toSkip = 0;
            int count = user.Sayings != null ? user.Sayings.Count : 0;
            if (count > 1)
            {
                toSkip = RandomNumberGenerator.GetInt32(count);
            }
            Saying saying = user.Sayings.Skip(toSkip).FirstOrDefault();

            return saying;
        }

        public async Task<Saying> GetRandomSayingAsync()
        {
            // Get random saying
            int toSkip = 0;
            int count = await _dbContext.Sayings.CountAsync();
            if (count > 1)
            {
                toSkip = RandomNumberGenerator.GetInt32(count);
            }

            return _dbContext.Sayings.Skip(toSkip).FirstOrDefault();
            
        }

        public async Task<List<Saying>> GetAllIsmsAsync(string ism)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.IsmKey.Equals(ism));
            if(user == null)
            {
                Log.Error("Error getting all sayings. User {0} was not found", ism);
                return new List<Saying>();
            }

            await _dbContext.Entry(user).Collection(u => u.Sayings).LoadAsync();

            return user.Sayings.ToList();
        }
    }
}

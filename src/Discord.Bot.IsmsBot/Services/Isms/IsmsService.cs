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
    public class IsmsService
    {
        private readonly UserSayingsContext _dbContext;
        private const string ismPattern = "(?<ismKey>[\\s\\S]+ism)\\s\"(?<ism>[\\s\\S]+)\"";

        public IsmsService(
            UserSayingsContext dbContext
            )
        {
            _dbContext = dbContext;
        }


        public async Task<Saying> AddIsmAsync(string commandString, SocketCommandContext discordContext)
        {
            Log.Verbose("Adding saying for `{0}`", commandString);
            Saying saying = null;
            if (!string.IsNullOrWhiteSpace(commandString))
            {
                // match the regex pattern for the command string `userism "phrase"`
                var match = Regex.Match(commandString, ismPattern);

                if (!match.Success)
                {
                    Log.Verbose("Pattern '{0}' did not match '{1}'", ismPattern, commandString);
                    return null;
                }

                string ismKey = match.Groups["ismKey"].Value.ToLower();
                string ism = match.Groups["ism"].Value;
                try
                {
                    // Try to get the userism from the database
                    saying = await _dbContext.Sayings.FirstOrDefaultAsync(s => s.IsmKey == ismKey && EF.Functions.Like(ism, s.IsmSaying) && s.GuildId == discordContext.Guild.Id);

                }
                catch (Exception ex)
                {
                    Log.Error("Error getting user {0}", ex);
                    throw;
                }

                // If the saying doesn't exist, create the new userism
                if (saying == null)
                {
                    saying = new Saying()
                    {
                        GuildId = discordContext.Guild.Id,
                        IsmKey = ismKey,
                        DateCreated = DateTime.Now,
                        IsmRecorder = discordContext.User.Username,
                        IsmSaying = ism
                    };

                    Log.Debug("Adding new user to database: {0} on server {1}", saying.IsmKey, discordContext.Guild.Id);

                    _dbContext.Sayings.Add(saying);
                }
                else
                {
                    // if the saying exists, don't create a new one
                    Log.Warning("This userism {0} already exists.", ism);
                    throw new Exception("The userism already exists for that user on this server");
                }

                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Log.Error(e, "An error occurred while adding an ism.");
                }
            }

            return saying;
        }

        public async Task<Saying> GetIsmAsync(string ismKey, SocketCommandContext discordContext)
        {
            ismKey = ismKey.ToLower();

            var query =  _dbContext
                .Sayings
                .Where(s => s.GuildId == discordContext.Guild.Id && ismKey == s.IsmKey);

            var count = query.Count();

            if (count == 0)
            {
                Log.Information("{0} doesn't seem to have any isms on this server yet.", ismKey);
                return null;
            }

            // Get random saying
            int toSkip = 0;
            if (count > 1)
            {
                toSkip = RandomNumberGenerator.GetInt32(count);
            }
            Saying saying = query.Skip(toSkip).FirstOrDefault();

            return saying;
        }

        public async Task<Saying> GetRandomSayingAsync(SocketCommandContext context)
        {
            // Get random saying
            int toSkip = 0;
            int count = await _dbContext.Sayings.CountAsync();
            if (count > 1)
            {
                toSkip = RandomNumberGenerator.GetInt32(count);
            }

            return _dbContext.Sayings.Where(s => s.GuildId == context.Guild.Id).Skip(toSkip).FirstOrDefault();

        }

        public async Task<List<Saying>> GetAllIsmsAsync(string ismKey, SocketCommandContext context)
        {
            var sayings = await _dbContext.Sayings.Where(s => s.IsmKey.Equals(ismKey) && s.GuildId == context.Guild.Id).ToListAsync();
            if (sayings == null)
            {
                Log.Error("Error getting all sayings. User {0} doesn't seem to have any sayings on this server yet.", ismKey);
                return new List<Saying>();
            }
            return sayings;
        }
    }
}

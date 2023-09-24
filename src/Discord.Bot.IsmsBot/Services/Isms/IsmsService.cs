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
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualBasic;

namespace Discord.Bot.IsmsBot
{
    public class IsmsService
    {
        private readonly UserSayingsContext _dbContext;


        private static readonly List<char> left_quote_characters = new List<char>() {
            '\u0022', // QUOTATION MARK
            '\u201C', // LEFT DOUBLE QUOTATION MARK
          };
        private static readonly List<char> right_quote_characters = new List<char>() {
            '\u0022', // QUOTATION MARK
            '\u201D'  // RIGHT DOUBLE QUOTATION MARK
        };

        private static readonly string ismPattern = $"(?<ismKey>[\\s\\S]+ism)\\s+[{String.Join("", left_quote_characters)}](?<ism>[\\s\\S]+)[{String.Join("", right_quote_characters)}]";

        public IsmsService(
            UserSayingsContext dbContext
            )
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Add an ism saying for a user on a server
        /// </summary>
        /// <param name="commandString"></param>
        /// <param name="discordContext"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
                    throw;
                }
            }

            return saying;
        }

        /// <summary>
        /// Get a random saying from a user from a server
        /// </summary>
        /// <param name="ismKey"></param>
        /// <param name="discordContext"></param>
        /// <returns></returns>
        public async Task<Saying> GetIsmAsync(string ismKey, SocketCommandContext discordContext)
        {
            ismKey = ismKey.ToLower();

            var query = _dbContext
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

        /// <summary>
        /// Get a random saying from the list of sayings on a server
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<Saying> GetRandomSayingAsync(SocketCommandContext context)
        {
            // Get random saying
            int toSkip = 0;
            IQueryable<Saying> query = _dbContext.Sayings.Where(s => s.GuildId == context.Guild.Id);
            int count = await query.CountAsync();
            if (count > 1)
            {
                toSkip = RandomNumberGenerator.GetInt32(count);
            }
            var saying = query.Skip(toSkip).FirstOrDefault();
            return saying;
        }

        /// <summary>
        /// Retrieve all ism sayings for a user
        /// </summary>
        /// <param name="ismKey"></param>
        /// <param name="context"></param>
        /// <returns></returns>
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

        public async Task<List<string>> GetAllIsmKeysForServerAsync(SocketCommandContext context)
        {
            var ismKeys = await _dbContext.Sayings.Where(s => s.GuildId == context.Guild.Id).Select(s => s.IsmKey).Distinct().ToListAsync();
            if (ismKeys == null)
            {
                string msg = "Error getting all ism keys for this server. Perhaps there are no sayings on this server yet.";
                Log.Information(msg);
                throw new InvalidOperationException(msg);
            }
            return ismKeys;
        }

        public string GetHelpAsync()
        {
            string msg = """
                # IsmsBot Help
                ## A bot designed to store and retrieve whacky sayings taken out of context
                User isms are stored per-server and are unique. Two users on the same server cannot have the same ism. 
                ### Authors 
                - TJ Price-Gedrites
                - Tyler Dalbora
                - Alex Parker

                ### List Of Commands
                - `!add`
                   - Used to add new 'isms' to the bot. Add is used on an ismkey followed by the saying in doublequotes. 
                   The doublequotes are required.
                   - SYNTAX: `!add <USER_ISM> "<USER_SAYING>"`
                   - EXAMPLE: `!add tjism "This is a saying that will be stored and retrieved later!"`
                - `!<USER>ism`
                   - Used to retrieve a saying via an ismkey. An ism key is of the form: \<user\>ism.
                   - SYNTAX: `!<user>ism`
                   - EXAMPLE: `!tjism` --> "This is a saying that will be stored and retrieved later!" - TJ
                - `!<USER>ism list`
                   - Used to list all saying associated with this user ism key.
                   - SYNTAX: `!<user>ism list`
                   - EXAMPLE: `!tjism list` --> "This is a saying that will be stored and retrieved later!" - TJ | Added by \<USER\> on \<DATE\>
                - `!random`
                   - Used to retrieve a random saying from any user
                   - SYNTAX: `!random`
                   - EXAMPLE `!random` --> "This is a random saying from a random user" - tyler | Added by TJ on \<DATE\>
                """;
            return msg;
        }
    }
}

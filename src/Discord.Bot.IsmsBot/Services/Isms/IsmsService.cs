﻿using Discord.Bot.Database;
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
using Discord.Bot.Database.Repositories;

namespace Discord.Bot.IsmsBot
{
    public class IsmsService
    {
        private static readonly List<char> left_quote_characters = new List<char>() {
            '\u0022', // QUOTATION MARK
            '\u201C', // LEFT DOUBLE QUOTATION MARK
          };
        private static readonly List<char> right_quote_characters = new List<char>() {
            '\u0022', // QUOTATION MARK
            '\u201D'  // RIGHT DOUBLE QUOTATION MARK
        };

        private static readonly string ismPattern = $"(?<ismKey>[\\s\\S]+ism)\\s+[{String.Join("", left_quote_characters)}](?<ism>[\\s\\S]+)[{String.Join("", right_quote_characters)}]";
        private SayingRepository _sayingsRepo;

        public IsmsService(
            SayingRepository sayingRepository
            )
        {
            _sayingsRepo = sayingRepository;
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
                    saying = await _sayingsRepo.GetSayingAsync(ismKey, ism, discordContext.Guild.Id);

                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error getting user saying");
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
                    try
                    {
                        await _sayingsRepo.AddIsmAsync(saying);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "An error occurred while adding an ism.");
                        throw;
                    }
                }
                else
                {
                    // if the saying exists, don't create a new one
                    Log.Warning("This userism {0} already exists.", ism);
                    throw new Exception("The userism already exists for that user on this server");
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
            return await _sayingsRepo.GetRandomIsmAsync(ismKey, discordContext.Guild.Id);
        }

        /// <summary>
        /// Get a random saying from the list of sayings on a server
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<Saying> GetRandomSayingAsync(SocketCommandContext context)
        {
            // Get random saying
            return await _sayingsRepo.GetRandomIsmAsync(context.Guild.Id);
        }

        /// <summary>
        /// Retrieve all ism sayings for a user
        /// </summary>
        /// <param name="ismKey"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<List<Saying>> GetAllIsmsAsync(string ismKey, SocketCommandContext context)
        {
            return await _sayingsRepo.GetAllIsmsAsync(ismKey, context.Guild.Id);
        }

        /// <summary>
        /// Get all the unique ism keys for the server.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<List<string>> GetAllIsmKeysForServerAsync(SocketCommandContext context)
        {
            return await _sayingsRepo.GetAllIsmKeysForServerAsync(context.Guild.Id);
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
                - `!listallkeys`
                   - Used to retrieve all ism keys from a server.
                   - SYNTAX `!listallkeys`
                   - EXAMPLE    `!listallkeys` --> 
                                Here is a list of all the isms on this server:
                                tylerism
                                tjism
                                alexism
                """;
            return msg;
        }
    }
}

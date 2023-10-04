using Discord.Bot.Database.Models;
using Discord.Bot.IsmsBot.WebUI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Discord.Bot.Database.Repositories
{
    public class SayingRepository : DBServiceBase
    {
        public SayingRepository(AppDBContext db, SemaphoreSlim ss) : base(db, ss)
        {
        }

        /// <summary>
        /// Get a specific ism from a specific user from a specific server asynchronously
        /// </summary>
        /// <param name="ismKey"></param>
        /// <param name="ism"></param>
        /// <param name="guildID"></param>
        /// <returns></returns>
        public async Task<Saying> GetSayingAsync(string ismKey, string ism, ulong guildID)
        {
            return await this.ShieldDb(async (db) =>
            {
                return await db.Sayings.FirstOrDefaultAsync(s => s.IsmKey == ismKey && EF.Functions.Like(ism, s.IsmSaying) && s.GuildId == guildID);
            });
        }

        public async Task AddIsmAsync(Saying saying)
        {
            await this.ShieldDb(async (db) =>
            {
                db.Sayings.Add(saying);
                await db.SaveChangesAsync();
            });
        }

        /// <summary>
        /// Get a random saying for a specific user on a specific server
        /// </summary>
        /// <param name="ismKey"></param>
        /// <param name="guildID"></param>
        /// <returns></returns>
        public async Task<Saying> GetRandomIsmAsync(string ismKey, ulong guildID)
        {
            return await this.ShieldDb(async (db) =>
            {
                var query = db.Sayings.Where(s => s.GuildId == guildID && ismKey == s.IsmKey);
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
                Saying saying = await query.Skip(toSkip).FirstOrDefaultAsync();
                return saying;
            });
        }

        /// <summary>
        /// Get a random saying from the list of sayings on a server
        /// </summary>
        /// <param name="guildID"></param>
        /// <returns></returns>
        public async Task<Saying> GetRandomIsmAsync(ulong guildID)
        {
            // Get random saying
            return await this.ShieldDb(async (db) =>
            {
                int toSkip = 0;
                IQueryable<Saying> query = db.Sayings.Where(s => s.GuildId == guildID);
                int count = await query.CountAsync();
                if (count > 1)
                {
                    toSkip = RandomNumberGenerator.GetInt32(count);
                }
                var saying = await query.Skip(toSkip).FirstOrDefaultAsync();
                return saying;
            });
        }

        /// <summary>
        /// Retrieve all ism sayings for a user
        /// </summary>
        /// <param name="ismKey"></param>
        /// <param name="guildID"></param>
        /// <returns></returns>
        public async Task<List<Saying>> GetAllIsmsAsync(string ismKey, ulong guildID)
        {
            return await this.ShieldDb(async (db) =>
            {
                var sayings = await db.Sayings.Where(s => s.IsmKey.Equals(ismKey) && s.GuildId == guildID).ToListAsync();
                if (sayings == null)
                {
                    Log.Error("Error getting all sayings. User {0} doesn't seem to have any sayings on this server yet.", ismKey);
                    return new List<Saying>();
                }
                return sayings;
            });
        }

        /// <summary>
        /// Get all the unique ism keys for the server.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<List<string>> GetAllIsmKeysForServerAsync(ulong guildID)
        {
            return await this.ShieldDb(async (db) =>
            {
                var ismKeys = await db.Sayings.Where(s => s.GuildId == guildID).Select(s => s.IsmKey).Distinct().ToListAsync();
                if (ismKeys == null)
                {
                    string msg = "Error getting all ism keys for this server. Perhaps there are no sayings on this server yet.";
                    Log.Warning(msg);
                    throw new ApplicationException(msg);
                }
                return ismKeys;
            });
        }

        /// <summary>
        /// Get a list of all servers from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Guild>> GetAllGuildsAsync()
        {
            return await this.ShieldDb(async (db) =>
            {
                return await db.Guilds.ToListAsync();
            });
        }


        /// <summary>
        /// Get all the sayings for all users for the server.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<List<Saying>> GetAllIsmsForServerAsync(ulong guildID)
        {
            return await this.ShieldDb(async (db) =>
            {
                var blah = await db.Sayings.Select(s => s.GuildId).ToListAsync();
                var isms = await db.Sayings.Where(s => s.GuildId == guildID).ToListAsync();
                if (isms == null)
                {
                    string msg = "Error getting all ism keys for this server. Perhaps there are no sayings on this server yet.";
                    Log.Warning(msg);
                    throw new ApplicationException(msg);
                }
                return isms;
            });
        }

    }
}

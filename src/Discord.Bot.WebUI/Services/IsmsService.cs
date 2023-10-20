using Discord.Bot.Database.Models;
using Discord.Bot.Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Discord.Bot.WebUI.Services
{
    /// <summary>
    /// API implementaion of the IsmService for use in the API to interact with the database
    /// </summary>
    public class IsmsService
    {
        private readonly SayingRepository _sayingsRepo;

        public IsmsService(SayingRepository sayingRepository)
        {
            _sayingsRepo = sayingRepository;
        }


        /// <summary>
        /// Get all servers from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<Guild>> GetAllGuildsAsync()
        {
            return await _sayingsRepo.GetAllGuildsAsync();
        }

        public async Task<List<Saying>> GetAllIsmsAsync(ulong guildId)
        {
            return await _sayingsRepo.GetAllIsmsForServerAsync(guildId);
        }

        public async Task DeleteIsmAsync(string ismID)
        {
            int response = await _sayingsRepo.DeleteIsmAsync(ismID);
            if(response < 1)
            {
                throw new DbUpdateException($"The DB deletion operation appears to have failed. {response} is less than 1.");
            }
        }

        public async Task<Saying> AddNewIsmAsync(Saying newIsm)
        {
            return await _sayingsRepo.AddIsmAsync(newIsm);
        }
    }
}
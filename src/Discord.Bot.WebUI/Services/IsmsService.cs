﻿using Discord.Bot.Database.Models;
using Discord.Bot.Database.Repositories;

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

    }
}
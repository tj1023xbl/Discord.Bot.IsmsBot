using Microsoft.AspNetCore.Mvc;
using Discord.Bot.Database.Models;
using Discord.Bot.WebUI.Services;

namespace Discord.Bot.WebUI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class IsmsController : Controller
    {
        private readonly IsmsService _ismsService;

        public IsmsController(IsmsService ismsService)
        {
            _ismsService = ismsService;
        }


        /* CRUD */

        // GET
        /// <summary>
        /// Get all guilds (servers) from the DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[Action]")]
        public async Task<List<GuildDTO>> GetAllGuildsAsync()
        {
            var blah = await _ismsService.GetAllGuildsAsync();
            return blah.Select(guild => new GuildDTO() { Id = $"{guild.Id}", Name = guild.Name }).ToList();
        }

        /// <summary>
        /// Get all sayings for all people on a server.
        /// </summary>
        /// <param name="guildId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[Action]/{guildId}")]
        public async Task<List<Saying>> GetAllSayingsAsync(string guildId)
        {
            ulong ulong_guildId = ulong.Parse(guildId);
            return await _ismsService.GetAllIsmsAsync(ulong_guildId);
        }

        // PUT

        // POST

        // DELETE
    }
}

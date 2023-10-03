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
        public async Task<List<Guild>> GetAllGuildsAsync()
        {
            return await _ismsService.GetAllGuildsAsync();
        }

        // PUT

        // POST

        // DELETE
    }
}

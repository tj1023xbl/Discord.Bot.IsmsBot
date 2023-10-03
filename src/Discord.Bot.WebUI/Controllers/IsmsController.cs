using Microsoft.AspNetCore.Mvc;
using Discord.Bot.Database.Models;

namespace Discord.Bot.WebUI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class IsmsController : Controller
    {
        /* CRUD */

        // GET
        /// <summary>
        /// Get all guilds (servers) from the DB
        /// </summary>
        /// <returns></returns>
        [Route("[Action]")]
        public async Task<List<Guild>> GetAllGuildsAsync()
        {
            throw new NotImplementedException();
        }

        // PUT

        // POST

        // DELETE
    }
}

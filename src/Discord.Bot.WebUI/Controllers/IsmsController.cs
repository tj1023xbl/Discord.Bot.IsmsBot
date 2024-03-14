using Microsoft.AspNetCore.Mvc;
using Discord.Bot.Database.Models;
using Discord.Bot.WebUI.Services;
using Serilog;

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
        public async Task<IActionResult> GetAllSayingsAsync(string guildId)
        {
            try
            {
                ulong ulong_guildId = ulong.Parse(guildId);
                var result =  await _ismsService.GetAllIsmsAsync(ulong_guildId);
                return new OkObjectResult(result);
            }
            catch(Exception ex)
            {
                Log.Error(ex, "An error occurred when getting all sayings from guild {0}", guildId);
                return new BadRequestObjectResult(new List<Saying>() { });
            }
        }

        // PUT
        [HttpPut("")]
        public async Task<IActionResult> EditIsmAsync(Saying newIsm)
        {
            try
            {
                Saying result = await _ismsService.EditIsmAsync(newIsm);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                Log.ForContext("NewIsm", newIsm, true).Error(ex, "An error occurred when trying to add ism '{0}'", newIsm.IsmSaying);
                return new BadRequestObjectResult(ex.Message);
            }
        }


        // POST
        [HttpPost("addnewism")]
        public async Task<IActionResult> AddNewIsmAsync(Saying newIsm)
        {
            try
            {
                Saying result = await _ismsService.AddNewIsmAsync(newIsm);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                Log.ForContext("NewIsm", newIsm, true).Error(ex, "An error occurred when trying to add ism '{0}'", newIsm.IsmSaying);
                return await Task.FromResult(new BadRequestObjectResult(ex.Message));
            }
        }

        // DELETE
        [HttpDelete]
        [Route("{ismId}")]
        public async Task<IActionResult> DeleteIsmFromServer([FromRoute]string IsmID)
        {
            try
            {
                await _ismsService.DeleteIsmAsync(IsmID);
                return new OkResult();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred when trying to delete ism {0}", IsmID);
                return await Task.FromResult(new BadRequestObjectResult(ex.Message));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Bot.IsmsBot
{
    public interface IDiscordProxy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task RunDiscordAppAsync();
    }
}

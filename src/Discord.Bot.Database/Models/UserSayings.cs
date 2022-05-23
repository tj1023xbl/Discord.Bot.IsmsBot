using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Discord.Bot.Database
{
    public partial class UserSayings
    {
        /// <summary>
        /// The name of the user containing these isms.
        /// </summary>
        [Key]
        public string IsmKey { get; set; }
        
        /// <summary>
        /// The list of sayings for this user.
        /// </summary>
        public List<string> Sayings { get; set; }

        /// <summary>
        /// The ID of the parent server object.
        /// </summary>
        public ulong GuildId { get; set; }

        /// <summary>
        /// The discord server this saying belongs to.
        /// </summary>
        public DiscordServer Server { get; set; }
    }
}

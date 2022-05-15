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
        public string UserIsm { get; set; }
        
        /// <summary>
        /// The list of sayings for this user.
        /// </summary>
        public List<string> Sayings { get; set; }

        /// <summary>
        /// The discord server this saying belongs to.
        /// </summary>
        public DiscordServer Server { get; set; }
    }
}

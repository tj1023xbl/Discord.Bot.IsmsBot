using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Discord.Bot.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Discord.Bot.Database
{
    [Index(nameof(GuildId), nameof(Username), IsUnique = true)]
    public partial class User
    {
        /// <summary>
        /// The name of the user containing these isms.
        /// </summary>
        [Key]
        public string Username { get; set; }
        
        /// <summary>
        /// The list of sayings for this user.
        /// </summary>
        public ICollection<Saying> Sayings { get; set; }

        /// <summary>
        /// The ID of the Guild this user belongs to.
        /// User isms are unique across guilds.
        /// </summary>
        public ulong GuildId { get; set; }

    }
}

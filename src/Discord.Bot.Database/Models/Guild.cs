using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Bot.Database.Models
{
    public class Guild
    {
        /// <summary>
        /// The ID of the discord server
        /// </summary>
        [Key]
        public ulong Id { get; set; }

        /// <summary>
        /// The name of the discord server
        /// </summary>
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Bot.Database
{
    public partial class DiscordServer
    {
        [Key]
        public Guid Id { get; set; }

        public string ServerName { get; set; }

        public string GuildId { get; set; }

        public List<UserSayings> UserSayings { get; set; }
    }
}

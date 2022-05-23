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
        public ulong Id { get; set; }

        public string ServerName { get; set; }

        public List<UserSayings> UserSayings { get; set; }
    }
}

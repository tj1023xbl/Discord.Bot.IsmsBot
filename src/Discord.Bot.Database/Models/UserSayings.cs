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
        public List<string> Sayings { get; set; }

        public string Test { get; set; }
    }
}

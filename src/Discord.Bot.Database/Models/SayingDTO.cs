using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Bot.Database.Models
{
    public class SayingDTO
    {
        /// <summary>
        /// The primary key for this Saying
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the user containing these isms.
        /// </summary>
        public string IsmKey { get; set; }


        /// <summary>
        /// The actual ism saying
        /// </summary>
        public string IsmSaying { get; set; }

        /// <summary>
        /// The date and time when the ism was recorded
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The username for the person who added this ism record
        /// </summary>
        public string IsmRecorder { get; set; }

        /// <summary>
        /// The ID of the Guild this user belongs to.
        /// User isms are unique across guilds.
        /// </summary>
        public string GuildId { get; set; }

    }
}

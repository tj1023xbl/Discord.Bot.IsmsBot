using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Discord.Bot.Database.Models
{
    /// <summary>
    /// The actual record for the user ism. 
    /// This is the class that stores the ism, as well as metadata about the ism.
    /// Isms are unique among users; they cannot have the same ism. 
    /// </summary>
    [Index(nameof(IsmSaying), IsUnique = true)]
    [Index(nameof(IsmKey), nameof(GuildId), IsUnique = false)]
    public class Saying
    {
        /// <summary>
        /// The primary key for this Saying
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the user containing these isms.
        /// </summary>
        [Required]
        public string IsmKey { get; set; }


        /// <summary>
        /// The actual ism saying
        /// </summary>
        [Required]
        public string IsmSaying { get; set; }

        /// <summary>
        /// The date and time when the ism was recorded
        /// </summary>
        [Required]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The username for the person who added this ism record
        /// </summary>
        [Required]
        public string IsmRecorder { get; set; }

        /// <summary>
        /// The ID of the Guild this user belongs to.
        /// User isms are unique across guilds.
        /// </summary>
        [Required]
        public ulong GuildId { get; set; }

    }
}

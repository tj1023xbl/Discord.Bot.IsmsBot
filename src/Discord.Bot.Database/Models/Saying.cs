using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Discord.Bot.Database.Models
{
    /// <summary>
    /// The actual record for the user ism. 
    /// This is the class that stores the ism, as well as metadata about the ism.
    /// Isms are unique among users; they cannot have the same ism twice. 
    /// </summary>
    [Index(nameof(Username), nameof(IsmSaying), IsUnique = true)]
    public class Saying
    {
        /// <summary>
        /// The foreign key to the user who owns this saying
        /// </summary>
        [ForeignKey("User")]
        public string Username { get; set; }

        /// <summary>
        /// The actual ism saying
        /// </summary>
        [Required]
        public string IsmSaying { get; set; }

        /// <summary>
        /// The date and time when the ism was recorded
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The username for the person who added this ism record
        /// </summary>
        public string IsmRecorder { get; set; }


    }
}

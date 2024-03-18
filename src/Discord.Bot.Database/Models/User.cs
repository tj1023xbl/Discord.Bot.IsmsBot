using Microsoft.AspNetCore.Identity;

namespace Discord.Bot.Database.Models
{

    public class User : IdentityUser
    {
        public User(string userName)
            : base()
        {
            UserName = userName;
        }
    }
}

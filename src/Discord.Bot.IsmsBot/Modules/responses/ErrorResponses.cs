using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Bot.IsmsBot
{
    public class ErrorResponses
    {
        public static string AddIsmsError = "There was an error adding your ism. Please try again and match the following format: \n ![cmdGroup] [command] [ism key] \"[ism]\"";
        public static string AddIsmModalError = "There was an error adding your ism. Please ensure all fields in are filled out, and the ism key ends with 'ism'";
    }
}

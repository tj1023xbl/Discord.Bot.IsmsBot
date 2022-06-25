using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IsmsBot.RegexCommand
{
    public class CommandOptions
    {
        public string Prefix { get; set; } = "!";
        public bool AcceptMentionPrefix { get; set; } = true;
        public bool AcceptBotMessages { get; set; } = false;
        public bool RequirePublicMessagePrefix { get; set; } = true;    // if false, prefix won't be required in guild channels
        public bool RequirePrivateMessagePrefix { get; set; } = true;  // if false, prefix won't be required in private messages

        public bool CaseSensitive { get; set; } = false;
        public RunMode DefaultRunMode { get; set; } = RunMode.Default;
        public bool IgnoreExtraArgs { get; set; } = true;

        // for loading
        public ICollection<Type> Classes { get; set; } = new List<Type>();  // classes that Regex Command System will look at when initializing
        public ICollection<Assembly> Assemblies { get; set; } = new List<Assembly>() { Assembly.GetEntryAssembly() };   // assemblies that Regex Command System will look at when initializing
    }
}

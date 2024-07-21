using Discord.Bot.Database.Models;
using Discord.Interactions;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Discord.Bot.IsmsBot
{
    public class IsmsModalModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IsmsService _ismsService;

        /// <summary>
        /// Constructor
        /// </summary>
        public IsmsModalModule(IsmsService ismsService)
        {
            _ismsService = ismsService;
        }

        [SlashCommand("addism", "Adds an ism message")]
        public async Task AddIsmModalAsync()
        {
            ModalBuilder mb = new ModalBuilder().WithTitle("Add an Ism!").
                        WithCustomId("add_ism_modal").
                        AddTextInput("Who's ism?", "ism_key", placeholder: "<user>ism", required: true).
                        AddTextInput("What did they say?", "ism_value", TextInputStyle.Paragraph, placeholder: "This is where I would put my quote. IF I HAD ONE!", required: true);

            await Context.Interaction.RespondWithModalAsync(mb.Build());
        }

        [ModalInteraction("add_ism_modal")]
        public async Task HandleIsmModal(string ism_key, string ism_value, SocketInteractionContext context) 
        {
            if(string.IsNullOrWhiteSpace(ism_key) || string.IsNullOrWhiteSpace(ism_value) || !ism_key.EndsWith("ism", System.StringComparison.InvariantCultureIgnoreCase))
                await Context.Channel.SendMessageAsync(ErrorResponses.AddIsmModalError);

            Saying user = await _ismsService.AddIsmAsync(ism_key, ism_value, context.Guild.Id, context.User.Username);

            if (user != null)
                await Context.Channel.SendMessageAsync($"Successfully added new saying for {user.IsmKey}");
            else
                await Context.Channel.SendMessageAsync(ErrorResponses.AddIsmModalError);
        }

    }
}

using Discord.Bot.Database.Models;
using Discord.Interactions;
using Serilog;
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

        public class IsmModal : IModal
        {
            public string Title => "Add an Ism!";
            public string CustomId => "add_ism_modal";

            [InputLabel("Who's Ism?")]
            [ModalTextInput("ism_key", placeholder: "<user>ism", minLength: 4)]
            public string IsmKey { get; set; }

            [InputLabel("What did they say?")]
            [ModalTextInput("ism_value", TextInputStyle.Paragraph, placeholder: "This is where I would put my quote. IF I HAD ONE!", minLength: 4)]
            public string IsmValue { get; set; }
        }

        [ModalInteraction("add_ism_modal")]
        public async Task ModalResponse(IsmModal ismModal)
        {
            Log.Debug("Received modal submission with <{0}, {1}>", ismModal.IsmKey, ismModal.IsmValue);
            if (string.IsNullOrWhiteSpace(ismModal.IsmKey) || string.IsNullOrWhiteSpace(ismModal.IsmValue) || !ismModal.IsmKey.EndsWith("ism", System.StringComparison.OrdinalIgnoreCase))
                await Context.Channel.SendMessageAsync(ErrorResponses.AddIsmModalError);

            Saying saying = await _ismsService.AddIsmAsync(ismModal.IsmKey, ismModal.IsmValue, Context.Guild.Id, Context.User.Username);

            if (saying != null)
                await RespondAsync($"Successfully added new saying for {saying.IsmKey}: \"{saying.IsmSaying}\"");
            else
                await RespondAsync(ErrorResponses.AddIsmModalError);
        }

    }
}

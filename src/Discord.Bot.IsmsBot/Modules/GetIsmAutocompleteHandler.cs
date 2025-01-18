using Discord.Interactions;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discord.Bot.IsmsBot
{
    public partial class IsmsModalModule
    {
        public class GetIsmAutocompleteHandler : AutocompleteHandler
        {
            /// <summary>
            /// This dictionary will be used as a cache so that we don't 
            /// have to hit the database every time we trigger the autocorrect.
            /// ulong - GuildID
            /// List<string> - list of user ism keys 
            /// </summary>
            private static ConcurrentDictionary<ulong, List<string>> _perGuildUserIsmInMemoryCache = new();

            private static DateTime _cacheLastRefreshed = default;

            private const int _cachettl = 15;
            private readonly IsmsService _ismsService;

            public GetIsmAutocompleteHandler(IsmsService ismsService)
            {
                _ismsService = ismsService;
            }

            public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
            {
                // check the cache and if we need to, grab the ism keys from the database
                if (_cacheLastRefreshed == default || DateTime.Now - _cacheLastRefreshed >= TimeSpan.FromMinutes(_cachettl) || !_perGuildUserIsmInMemoryCache.ContainsKey(context.Guild.Id))
                {
                    // pull the isms from the database and save them to the cache
                    List<string> ismKeys = await _ismsService.GetAllIsmKeysForServerAsync(context.Guild);
                    _cacheLastRefreshed = DateTime.Now;
                    _perGuildUserIsmInMemoryCache.TryAdd(context.Guild.Id, ismKeys);
                    Log.Verbose("Cache refreshed");
                }

                // return the ism keys from the cache
                if (_perGuildUserIsmInMemoryCache.TryGetValue(context.Guild.Id, out var list))
                {

                    List<AutocompleteResult> results = list.Select(s =>
                    {
                        return new AutocompleteResult(s, s);
                    }).ToList();
                    results.Add(new AutocompleteResult("Random", "Random"));
                    return AutocompletionResult.FromSuccess(results);
                }
                else
                {
                    // log an error
                    Log.Error("Unable to grab the ism keys for {0} from the cache", context.Guild.Name);
                    // return from DB anyway
                    var ismKeys = await _ismsService.GetAllIsmKeysForServerAsync(context.Guild);
                    IEnumerable<AutocompleteResult> results = list.Select(s =>
                    {
                        return new AutocompleteResult(s, s);
                    });
                    return AutocompletionResult.FromSuccess(results);
                }

            }
        }
    }
}

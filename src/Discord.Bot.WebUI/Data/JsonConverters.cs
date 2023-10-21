using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.Bot.WebUI.Data.JsonConverters
{
    public class ULongJsonConverter : JsonConverter<ulong>
    {
        public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            bool success = ulong.TryParse(str, out ulong result);
            if(!success)
            {
                throw new Exception($"Unable to parse ulong {str}");
            }
            return result;
        }

        public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(string.Format("{0}", value));
        }
    }
}

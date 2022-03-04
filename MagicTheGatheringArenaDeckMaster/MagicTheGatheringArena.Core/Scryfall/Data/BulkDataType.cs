using Newtonsoft.Json;

namespace MagicTheGatheringArena.Core.Scryfall.Data
{
    public class BulkDataType
    {
        [JsonProperty("object")]
        public string @object { get; set; }

        [JsonProperty("id")]
        public string id_field { get; set; }

        [JsonProperty("type")]
        public string type_field { get; set; }

        public string updated_at { get; set; }
        public string uri { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string compressed_size { get; set; }
        public string download_uri { get; set; }
        public string content_type { get; set; }
        public string content_encoding { get; set; }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;

namespace MagicTheGatheringArena.Core.MTGA.Data
{
    public class LocalizationType
    {
        public string isoCode { get; set; }

        [JsonProperty("keys")]
        List<StringType> keys { get; set; }

        public LocalizationType()
        {
            keys = new List<StringType>();
        }
    }
}

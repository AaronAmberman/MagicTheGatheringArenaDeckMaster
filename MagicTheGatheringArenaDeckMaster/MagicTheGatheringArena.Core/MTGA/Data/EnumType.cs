using Newtonsoft.Json;
using System.Collections.Generic;

namespace MagicTheGatheringArena.Core.MTGA.Data
{
    public class EnumType
    {
        public string name { get; set; }

        [JsonProperty("values")]
        List<ValueType> values { get; set; }

        public EnumType()
        {
            values = new List<ValueType>();
        }
    }
}

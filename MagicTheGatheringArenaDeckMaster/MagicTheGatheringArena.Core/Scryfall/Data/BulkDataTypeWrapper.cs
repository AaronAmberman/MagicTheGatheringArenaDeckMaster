using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicTheGatheringArena.Core.Scryfall.Data
{
    public class BulkDataTypeWrapper
    {
        [JsonProperty("object")]
        public string @object { get; set; }

        public bool has_more { get; set; }

        [JsonProperty("data")]
        public List<BulkDataType> data { get; set;}

        public BulkDataTypeWrapper()
        {
            data = new List<BulkDataType>();
        }
    }
}

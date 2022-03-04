using Newtonsoft.Json;
using System.Collections.Generic;

namespace MagicTheGatheringArena.Core.MTGA.Data
{
    public class AbilityType
    {
        public int id { get; set; }
        public int baseId { get; set; }
        public int text { get; set; }
        public int category { get; set; }

        [JsonProperty("relevantZones")]
        public List<int> relevantZones { get; set; }

        public int subCategory { get; set; }
        public int requiresConfirmation { get; set; }
        
        [JsonProperty("referencedKeywordTypes")]
        public List<int> referencedKeywordTypes { get; set; }

        [JsonProperty("referencedKeywords")]
        public List<int> referencedKeywords { get; set; }

        [JsonProperty("paymentTypes")]
        public List<int> paymentTypes { get; set; }

        public string manaCost { get; set; }
        public int abilityWord { get; set; }

        [JsonProperty("modalAbilityChildren")]
        public List<int> modalAbilityChildren { get; set; }

        [JsonProperty("linkedHiddenAbilities")]
        public List<int> linkedHiddenAbilities { get; set; }

        public AbilityType()
        {
            relevantZones = new List<int>();
            referencedKeywordTypes = new List<int>();
            referencedKeywords = new List<int>();
            paymentTypes = new List<int>();
            modalAbilityChildren = new List<int>();
            linkedHiddenAbilities = new List<int>();
        }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;

namespace MagicTheGatheringArena.Core.MTGA.Data
{
    public class CardType
    {
        public int grpid { get; set; }
        public int titleId { get; set; }
        public int artId { get; set; }
        public string power { get; set; }
        public string toughness { get; set; }
        public int flavorId { get; set; }
        public string collectorNumber { get; set; }
        public string collectorMax { get; set; }
        public int cmc { get; set; }
        public int rarity { get; set; }
        public int altDeckLimit { get; set; }
        public string artistCredit { get; set; }
        public string set { get; set; }

        [JsonProperty("types")]
        public List<int> types { get; set; }

        [JsonProperty("subtypes")]
        public List<int> subtypes { get; set; }

        public int cardTypeTextId { get; set; }
        public int subtypeTextId { get; set; }

        [JsonProperty("colors")]
        public List<int> colors { get; set; }

        [JsonProperty("frameColors")]
        public List<int> frameColors { get; set; }

        [JsonProperty("colorIdentity")]
        public List<int> colorIdentity { get; set; }

        public string castingcost { get; set; }

        [JsonProperty("abilities")]
        List<ValueType> abilities { get; set; }

        [JsonProperty("knownSupportedStyles")]
        List<string> knownSupportedStyles { get; set; }

        public string DigitalReleaseSet { get; set; }

        public CardType()
        {
            types = new List<int>();
            subtypes = new List<int>();
            colors = new List<int>();
            frameColors = new List<int>();
            colorIdentity = new List<int>();
            abilities = new List<ValueType>();
            knownSupportedStyles = new List<string>();
        }
    }
}

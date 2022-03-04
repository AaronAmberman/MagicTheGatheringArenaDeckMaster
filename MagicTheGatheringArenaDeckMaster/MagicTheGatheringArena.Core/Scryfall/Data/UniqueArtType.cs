using Newtonsoft.Json;
using System.Collections.Generic;

namespace MagicTheGatheringArena.Core.Scryfall.Data
{
    public class UniqueArtType
    {
        [JsonProperty("object")]
        public string @object { get; set; }

        [JsonProperty("id")]
        public string id_field { get; set; }

        public string oracle_id { get; set; }

        [JsonProperty("multiverse_ids")]
        public List<int> multiverse_ids { get; set; }

        public int mtgo_id { get; set; }
        public int mtgo_foil_id { get; set; }
        public int tcgplayer_id { get; set; }
        public int cardmarket_id { get; set; }

        [JsonProperty("name")]
        public string name_field { get; set; }

        public string lang { get; set; }
        public string released_at { get; set; } // this is a date

        [JsonProperty("uri")]
        public string uri_field { get; set; }

        public string scryfall_uri { get; set; }
        public string layout { get; set; }
        public bool highres_image { get; set; }
        public string image_status { get; set; }
        public ImageUriType image_uris { get; set; }
        public string mana_cost { get; set; }
        public decimal cmc { get; set; }
        public string type_line { get; set; }
        public string oracle_text { get; set; }
        public string power { get; set; }
        public string toughness { get; set; }

        [JsonProperty("colors")]
        public List<string> colors { get; set; }

        [JsonProperty("colorIdentity")]
        public List<string> colorIdentity { get; set; }

        [JsonProperty("keywords")]
        public List<string> keywords { get; set; }

        public LegalityType legalities { get; set; }

        [JsonProperty("games")]
        public List<string> games { get; set; }

        public bool reserved { get; set; }
        public bool foil { get; set; }
        public bool nonfoil { get; set; }

        [JsonProperty("finishes")]
        public List<string> finishes { get; set; }

        public bool oversized { get; set; }
        public bool promo { get; set; }
        public bool reprint { get; set; }
        public bool variation { get; set; }
        public string set_id { get; set; }
        public string set { get; set; }
        public string set_name { get; set; }
        public string set_type { get; set; }
        public string set_uri { get; set; }
        public string set_search_uri { get; set; }
        public string scryfall_set_uri { get; set; }
        public string rulings_uri { get; set; }
        public string prints_search_uri { get; set; }
        public string collector_number { get; set; }
        public bool digital { get; set; }
        public string rarity { get; set; }
        public string flavor_text { get; set; }
        public string card_back_id { get; set; }
        public string artist { get; set; }

        [JsonProperty("artist_ids")]
        public List<string> artist_ids { get; set; }

        public string illustration_id { get; set; }
        public string border_color { get; set; }
        public string frame { get; set; }
        public bool full_art { get; set; }
        public bool textless { get; set; }
        public bool booster { get; set; }
        public bool story_spotlight { get; set; }
        public int edhrec_rank { get; set; }
        public PricesType prices { get; set; }
        public RelatedUriType related_uris { get; set; }

        public UniqueArtType()
        {
            multiverse_ids = new List<int>();
            colors = new List<string>();
            colorIdentity = new List<string>();
            keywords = new List<string>();
            games = new List<string>();
            finishes = new List<string>();
            artist_ids = new List<string>();
        }
    }
}

using System.Collections.Generic;

namespace MagicTheGatheringArena.Core.Database.Models
{
    public class Deck
    {
        public string GameType { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }

        public List<Card> Cards { get; set; } = new List<Card>();

        public int NumberOfCreatures => 0;
        public int NumberOfInstants => 0;
        public int NumberOfSorceries => 0;
        public int NumberOfEnchantments => 0;
        public int NumberOfArtifacts => 0;
        public int NumberOfLands => 0;
        public int TotalCards => 0;
    }
}

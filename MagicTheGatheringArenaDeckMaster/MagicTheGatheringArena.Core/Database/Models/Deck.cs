using System.Collections.Generic;

namespace MagicTheGatheringArena.Core.Database.Models
{
    public class Deck
    {
        public string GameType { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }

        public List<Card> Cards { get; set; } = new List<Card>();
    }
}

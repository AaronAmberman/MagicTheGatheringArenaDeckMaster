namespace MagicTheGatheringArena.Core.Database.Models
{
    public class Card
    {
        public long Id { get; set; }
        public long DeckId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string SetSymbol { get; set; }
        public int CardNumber { get; set; }
    }
}

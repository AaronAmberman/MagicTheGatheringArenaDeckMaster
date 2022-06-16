namespace MagicTheGatheringArenaDeckMaster.Models
{
    internal class Card
    {
        public int Id { get; set; }
        public int DeckId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string SetSymbol { get; set; }
        public int CardNumber { get; set; }
    }
}

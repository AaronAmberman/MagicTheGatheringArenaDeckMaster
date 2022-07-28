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
        public string Color { get; set; }
        public string Type { get; set; }

        public Card Clone()
        {
            return new Card
            {
                Id = Id,
                DeckId = DeckId,
                Name = Name,
                Count = Count,
                SetSymbol = SetSymbol,
                CardNumber = CardNumber,
                Color = Color,
                Type = Type
            };
        }
    }
}

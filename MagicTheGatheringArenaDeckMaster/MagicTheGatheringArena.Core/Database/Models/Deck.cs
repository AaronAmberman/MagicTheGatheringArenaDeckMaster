using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MagicTheGatheringArena.Core.Database.Models
{
    public class Deck
    {
        public string GameType { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }

        public List<Card> Cards { get; set; } = new List<Card>();

        public int NumberOfCreatures => Cards.Count(card => card.Type.Contains("Creature"));
        public int NumberOfInstants => Cards.Count(card => card.Type.Contains("Instant"));
        public int NumberOfSorceries => Cards.Count(card => card.Type.Contains("Sorcery"));
        public int NumberOfEnchantments => Cards.Count(card => card.Type.Contains("Enchantment"));
        public int NumberOfArtifacts => Cards.Count(card => card.Type.Contains("Artifact"));
        public int NumberOfLands => Cards.Count(card => card.Type.Contains("Land") && card.Type != "Artifact Land");
        public int TotalCards => Cards.Sum(card => card.Count);

        public Visibility HasWhite => Cards.Any(card => card.Color.Contains("W")) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility HasBlue => Cards.Any(card => card.Color.Contains("U")) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility HasBlack => Cards.Any(card => card.Color.Contains("B")) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility HasRed => Cards.Any(card => card.Color.Contains("R")) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility HasGreen => Cards.Any(card => card.Color.Contains("G")) ? Visibility.Visible : Visibility.Collapsed;

        public Deck Clone()
        {
            Deck clone = new Deck
            {
                GameType = GameType,
                Id = Id,
                Name = Name
            };

            foreach (Card card in Cards)
            {
                clone.Cards.Add(card.Clone());
            }

            return clone;
        }

        public void ZeroId()
        {
            Id = 0;

            foreach (Card card in Cards)
            {
                card.Id = 0;
            }
        }
    }
}

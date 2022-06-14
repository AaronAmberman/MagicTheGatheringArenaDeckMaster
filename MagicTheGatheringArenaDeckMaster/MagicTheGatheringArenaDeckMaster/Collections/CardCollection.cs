using MagicTheGatheringArenaDeckMaster.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MagicTheGatheringArenaDeckMaster.Collections
{
    /// <summary>A special collection for cards that can pull cards out by sets.</summary>
    internal class CardCollection : IEnumerable<IEnumerable<UniqueArtTypeViewModel>>
    {
        #region Fields

        private Dictionary<string, List<UniqueArtTypeViewModel>> dictionary = new Dictionary<string, List<UniqueArtTypeViewModel>>();

        #endregion

        #region Properties

        /// <summary>Get all cards within set (includes both normal and alchemic variations of cards).</summary>
        /// <param name="setName">The name of the set to get cards for.</param>
        /// <returns>The list of cards for the set.</returns>
        public List<UniqueArtTypeViewModel> this[string setName]
        {
            get
            {
                return dictionary[setName];
            }
        }

        /// <summary>Gets the cards within the set that match the given criteria.</summary>
        /// <param name="setName">The name of the set to get cards for.</param>
        /// <param name="alchemicVariation">True for cards that have alchemic variations, false for ones that are normal (do not have a gold A in the upper left).</param>
        /// <returns>The list of cards for the set that match the criteria.</returns>
        public List<UniqueArtTypeViewModel> this[string setName, bool alchemicVariation]
        {
            get
            {
                if (!alchemicVariation)
                {
                    List<UniqueArtTypeViewModel> variations = dictionary[setName].Where(item => item.IsAlchemyVariation).ToList();

                    return dictionary[setName].Except(variations).ToList();
                }
                else
                {
                    List<UniqueArtTypeViewModel> variations = dictionary[setName].Where(item => item.IsAlchemyVariation).ToList();
                    List<string> variantNames = variations.Select(item => item.Name.Replace("A-", "")).ToList();

                    // remove the non-alchemic variations
                    List<UniqueArtTypeViewModel> cardsToReturn = 
                        dictionary[setName].Where(item => !variantNames.Any(variantName => variantName == item.Name && 
                            !item.Name.StartsWith("A-", StringComparison.OrdinalIgnoreCase))).ToList();

                    return cardsToReturn;
                }
            }
        }

        #endregion

        #region Methods

        public void Add(UniqueArtTypeViewModel item)
        {
            // make the set card list to hold the card...if needed
            if (!dictionary.ContainsKey(item.Set))
            {
                dictionary.Add(item.Set, new List<UniqueArtTypeViewModel>());
            }

            //Debug.WriteLine($"Set: {item.Set} | Name: {item.Name} | Mana cost: {item.Model.mana_cost} | Type: {item.Model.type_line} | Number of colors: {item.Model.colors.Count}");

            dictionary[item.Set].Add(item);
            dictionary[item.Set] = dictionary[item.Set].OrderBy(x => x.NumberOfColors).ThenBy(x => x.ColorScore).ThenBy(x => x.ManaCostTotal).ThenBy(x => x.Name).ToList();
        }

        public void AddMany(List<UniqueArtTypeViewModel> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public bool Contains(UniqueArtTypeViewModel item)
        {
            if (!dictionary.ContainsKey(item.Model.set_name))
            {
                return dictionary[item.Model.set_name].Contains(item);
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.Values.GetEnumerator();
        }

        IEnumerator<IEnumerable<UniqueArtTypeViewModel>> IEnumerable<IEnumerable<UniqueArtTypeViewModel>>.GetEnumerator()
        {
            return dictionary.Values.GetEnumerator();
        }

        public void Remove(UniqueArtTypeViewModel item)
        {
            // make the set card list to hold the card...if needed
            if (!dictionary.ContainsKey(item.Model.set_name))
            {
                dictionary[item.Model.set_name].Remove(item);
            }
        }

        public void RemoveMany(List<UniqueArtTypeViewModel> items)
        {
            foreach (var item in items)
                Remove(item);
        }

        public void SortAll()
        {
            foreach (string setName in dictionary.Keys)
            {
                List<UniqueArtTypeViewModel> cards = dictionary[setName];

                dictionary[setName] = cards.OrderBy(x => x.Name).ToList();
            }
        }

        #endregion
    }
}

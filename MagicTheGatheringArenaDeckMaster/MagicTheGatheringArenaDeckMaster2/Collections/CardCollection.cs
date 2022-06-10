using MagicTheGatheringArenaDeckMaster2.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MagicTheGatheringArenaDeckMaster2.Collections
{
    internal class CardCollection : IEnumerable<IEnumerable<UniqueArtTypeViewModel>>
    {
        #region Fields

        private Dictionary<string, List<UniqueArtTypeViewModel>> dictionary = new Dictionary<string, List<UniqueArtTypeViewModel>>();

        #endregion

        #region Properties

        public List<UniqueArtTypeViewModel> this[string setName]
        {
            get
            {
                return dictionary[setName];
            }
        }

        #endregion

        #region Methods

        public void Add(UniqueArtTypeViewModel item)
        {
            // make the set card list to hold the card...if needed
            if (!dictionary.ContainsKey(item.Model.set_name))
            {
                dictionary.Add(item.Model.set_name, new List<UniqueArtTypeViewModel>());
            }

            // insert card
            dictionary[item.Model.set_name].Add(item);
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

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MagicTheGatheringArenaDeckMaster2
{
    public static class ExtensionMethods
    {
        #region ICollection<T>

        /// <summary>Adds the elements of the specified enumerable to the end of the collection.</summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to add items to.</param>
        /// <param name="enumerable">The items to add to the collection.</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> enumerable)
        {
            foreach (T t in enumerable)
            {
                collection.Add(t);
            }
        }

        /// <summary>Removes the specified elements from the collection.</summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to remove items from.</param>
        /// <param name="enumerable">The items to remove from the collection.</param>
        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> enumerable)
        {
            foreach (T t in enumerable)
            {
                collection.Remove(t);
            }
        }

        #endregion

        #region IList

        /// <summary>Adds the elements of the specified enumerable to the end of the list.</summary>
        /// <param name="list">The list to add items to.</param>
        /// <param name="enumerable">The items to add to the list.</param>
        public static void AddRange<T>(this IList list, IEnumerable<T> enumerable)
        {
            foreach (T t in enumerable)
            {
                list.Add(t);
            }
        }

        /// <summary>Removes the specified elements from the list.</summary>
        /// <param name="list">The list to remove items from.</param>
        /// <param name="enumerable">The items to remove from the list.</param>
        public static void RemoveRange<T>(this IList list, IEnumerable<T> enumerable)
        {
            foreach (T t in enumerable)
            {
                list.Remove(t);
            }
        }

        #endregion

        #region ObservableCollection<T>

        /// <summary>Adds the elements of the specified enumerable to the end of the collection.</summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to add items to.</param>
        /// <param name="enumerable">The items to add to the collection.</param>
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> enumerable)
        {
            foreach (T t in enumerable)
            {
                collection.Add(t);
            }
        }

        /// <summary>Removes the specified elements from the collection.</summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to remove items from.</param>
        /// <param name="enumerable">The items to remove from the collection.</param>
        public static void RemoveRange<T>(this ObservableCollection<T> collection, IEnumerable<T> enumerable)
        {
            foreach (T t in enumerable)
            {
                collection.Remove(t);
            }
        }

        #endregion

        #region String

        /// <summary>Removes <, >, :, ", /, \, |, ? and * from the string and replace them with a - (hyphen).</summary>
        /// <param name="str">The string to replace characters in.</param>
        /// <returns>The corrected string.</returns>
        public static string ReplaceBadWindowsCharacters(this string str)
        {
            return str.Replace("<", "-").Replace(">", "-").Replace(":", "-").Replace("/", "-")
                      .Replace("\\", "-").Replace("|", "-").Replace("?", "-").Replace("*", "-").Replace("//", "-");
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using UnityEditor.Localization;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Search;

namespace Agame.Localization
{
    public static class LocalizedStringPicker
    {
        private static List<SearchItem> searchItems = new List<SearchItem>();

        private static readonly SearchProvider searchProvider = new SearchProvider("agame-localized-string", "Localized Strings")
        {
            filterId = "loc:",
            priority = 999,

            fetchItems = (context, items, provider) =>
            {
                PopulateSearchItems(context.searchQuery);

                ///
                return searchItems;
            },

            fetchLabel = (item, context) =>
            {
                return item.label;
            },

            fetchDescription = (item, context) =>
            {
                return item.description;
            }
        };

        private class LocalizedStringSearchItem : SearchItem
        {
            private string collection;
            private string entry;
            private string englishText;

            private LocalizedString localizedString;

            public LocalizedString LocalizedString
            {
                get
                {
                    if (localizedString == null)
                    {
                        localizedString = new LocalizedString(collection, entry);
                    }
                    return localizedString;
                }
            }

            public LocalizedStringSearchItem(string collection, string entry, string englishText) : base((collection + entry).GetHashCode().ToString())
            {
                this.collection = collection;
                this.entry = entry;
                this.englishText = englishText;
                description = $"{collection}/{entry}";
                label = englishText;
            }
        }

        public static void ShowPicker(string searchText, Action<LocalizedString> onSelected)
        {
            var context = SearchService.CreateContext(searchProvider, searchText);

            ///
            SearchService.ShowPicker(
                context,
                (item, canceled) =>
                {
                    if (canceled || item == null)
                        return;

                    ///
                    if (onSelected != null)
                    {
                        var localizedStringSearchItem = item as LocalizedStringSearchItem;
                        var localizedString = localizedStringSearchItem.LocalizedString;
                        onSelected(localizedString);
                    }
                });
        }

        private static void PopulateSearchItems(string searchQuery)
        {
            ///
            searchItems.Clear();

            ///
            var collections = LocalizationEditorSettings.GetStringTableCollections();
            foreach (var collection in collections)
            {
                var englishTable = collection.GetTable("en") as StringTable;
                if (englishTable == null)
                    continue;
                foreach (var entry in englishTable.Values)
                {
                    if (entry.Value.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0
                        || entry.Key.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        var searchItem = new LocalizedStringSearchItem(collection.TableCollectionName, entry.Key, entry.Value);
                        searchItems.Add(searchItem);
                    }
                }
            }
        }
    }

}
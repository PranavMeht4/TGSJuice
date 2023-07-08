using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace TGSJuice
{
    public class AddJuiceSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private List<Type> _juiceTypes;
        private Dictionary<Type, string> juices = new Dictionary<Type, string>();

        private void OnEnable()
        {
            GetJuiceTypes();
        }

        private void GetJuiceTypes()
        {

            _juiceTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                           from assemblyType in domainAssembly.GetTypes()
                           where assemblyType.IsSubclassOf(typeof(TGSJuiceBase))
                           select assemblyType).ToList();

            foreach (var juiceType in _juiceTypes)
            {
                var juiceLabel = Attribute.GetCustomAttribute(juiceType, typeof(JuiceLabelAttribute)) as JuiceLabelAttribute;
                var label = juiceLabel != null ? juiceLabel.Label : juiceType.Name;
                juices.Add(juiceType, label);
            }
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry> { new SearchTreeGroupEntry(new GUIContent("Select juice to add"), 0) };

            var groupedJuices = juices.Values
                .Select(j => new { Juice = j, SplitIndex = j.IndexOf("/") })
                .GroupBy(j => j.SplitIndex == -1 ? string.Empty : j.Juice.Substring(0, j.SplitIndex))
                .ToList();

            foreach (var group in groupedJuices)
            {
                if (!string.IsNullOrEmpty(group.Key))
                {
                    entries.Add(new SearchTreeGroupEntry(new GUIContent(group.Key), 1));
                }

                foreach (var juice in group)
                {
                    string label = juice.SplitIndex == -1 ? juice.Juice : juice.Juice.Substring(juice.SplitIndex + 1);
                    var correspondingType = juices.First(kvp => kvp.Value == juice.Juice).Key;
                    entries.Add(new SearchTreeEntry(new GUIContent(label))
                    {
                        level = juice.SplitIndex == -1 ? 1 : 2,
                        userData = correspondingType
                    });
                }
            }

            return entries;
        }

        private static Action<Type> _onJuiceSelected;

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var selectedJuice = SearchTreeEntry.userData as Type;
            _onJuiceSelected?.Invoke(selectedJuice);
            return true;
        }

        internal static void OnClickJuiceType(Action<Type> onClick)
        {
            _onJuiceSelected = onClick;
        }
    }
}
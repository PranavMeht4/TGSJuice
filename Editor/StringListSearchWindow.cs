using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace TGSJuice
{
    public class StringListSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private static List<MonoBehaviour> _listeners = new List<MonoBehaviour>();
        private static System.Type currentActionType;

        public static void OpenSearchWindow(System.Type actionType)
        {
            currentActionType = actionType;
            _listeners.Clear();
            _listeners = FindObjectsOfType(typeof(Component)).OfType<MonoBehaviour>()
            .Where(monoBehaviour => actionType.IsInstanceOfType(monoBehaviour))
            .ToList();
            MySearchWindow.ShowWindow();
        }

        public static void RefreshSearchWindow()
        {
            _listeners.Clear();
            _listeners = FindObjectsOfType(typeof(Component)).OfType<MonoBehaviour>()
            .Where(monoBehaviour => currentActionType.IsInstanceOfType(monoBehaviour))
            .ToList();
            MySearchWindow.ShowWindow();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("List of listeners"), 0)
            };

            foreach (var listener in _listeners)
            {
                var newEntry = new SearchTreeEntry(new GUIContent($"  {listener.name}"))
                {
                    level = 1,
                    userData = listener
                };

                entries.Add(newEntry);
            }

            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Selection.activeGameObject = searchTreeEntry.userData as GameObject;
            return false;
        }

        public class MySearchWindow : EditorWindow
        {
            private static MySearchWindow _window;
            private Vector2 _scrollPosition = Vector2.zero;

            public static void ShowWindow()
            {
                _window = GetWindow<MySearchWindow>("All Listeners");
                _window.Focus();
            }

            private void OnGUI()
            {
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
                GUILayout.Space(20);
                foreach (var listener in StringListSearchWindow._listeners.Where(l => l != null).OrderBy(l => l.name))
                {
                    EditorGUILayout.ObjectField(listener.name, listener, typeof(GameObject), true);
                }
                GUILayout.EndScrollView();

                if (GUILayout.Button("Refresh"))
                {
                    _window.Close();
                    StringListSearchWindow.RefreshSearchWindow();
                }
            }
        }
    }
}
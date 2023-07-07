using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TGSJuice
{
    public class ListnerWindow : EditorWindow
    {
        private static List<MonoBehaviour> _listeners = new List<MonoBehaviour>();
        private static System.Type currentActionType;

        private static ListnerWindow _window;
        private Vector2 _scrollPosition = Vector2.zero;

        public static void ShowWindow(System.Type actionType = null)
        {
            if (actionType != null)
                currentActionType = actionType;

            if (currentActionType == null)
            {
                EditorUtility.DisplayDialog("Event Listening Status", "Event has no listeners or doesn't require them.", "OK");
                return;
            }

            _listeners.Clear();
            _listeners = FindObjectsOfType(typeof(Component)).OfType<MonoBehaviour>()
            .Where(monoBehaviour => actionType.IsInstanceOfType(monoBehaviour))
            .ToList();
            _window = GetWindow<ListnerWindow>("All Listeners");
            _window.Focus();
        }

        private void OnGUI()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            GUILayout.Space(20);
            foreach (var listener in _listeners.Where(l => l != null).OrderBy(l => l.name))
            {
                EditorGUILayout.ObjectField(listener, typeof(GameObject), true);
            }
            GUILayout.EndScrollView();

            if (GUILayout.Button("Refresh"))
            {
                _window.Close();
                RefreshSearchWindow();
            }
        }

        public static void RefreshSearchWindow()
        {
            _listeners.Clear();
            _listeners = FindObjectsOfType(typeof(Component)).OfType<MonoBehaviour>()
            .Where(monoBehaviour => currentActionType.IsInstanceOfType(monoBehaviour))
            .ToList();
            ShowWindow();
        }
    }
}
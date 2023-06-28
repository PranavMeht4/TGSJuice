using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TGSJuice
{
    [CustomEditor(typeof(TGSJuices))]
    public class TGSActionHierarchyHighlight : Editor
    {
        public static List<GameObject> ObjectsToHighlight = new List<GameObject>();

        private void OnEnable()
        {
            EditorApplication.hierarchyWindowItemOnGUI += hierarchWindowOnGUI;
        }

        private void OnDisable()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= hierarchWindowOnGUI;
        }

        private void hierarchWindowOnGUI(int instanceID, Rect selectionRect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            Debug.Log(ObjectsToHighlight.Count);
            foreach (var gameObject in ObjectsToHighlight)
            {
                if (gameObject == obj)
                {
                    EditorGUI.DrawRect(selectionRect, new Color(0, 0, 0, .25f));
                }
            }
        }

        internal static void HighlightReferences(List<GameObject> gameObjects)
        {
            ObjectsToHighlight = gameObjects.ToList();
        }
    }
}
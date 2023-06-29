using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace TGSJuice
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TGSJuices))]
    public class TGSJuicesEditor : Editor
    {
        private List<Type> _juiceTypes;
        private static Dictionary<int, Editor> _juiceEditors = new Dictionary<int, Editor>();
        private static Dictionary<int, bool> _juiceFoldouts = new Dictionary<int, bool>();

        private void OnEnable()
        {
            FindJuiceTypes();
            // EditorApplication.hierarchyWindowItemOnGUI += HierarchWindowOnGUI;
        }

        // private void OnDisable()
        // {
        //     EditorApplication.hierarchyWindowItemOnGUI -= HierarchWindowOnGUI;
        // }

        public override void OnInspectorGUI()
        {
            TGSJuices target = (TGSJuices)base.target;

            foreach (var juice in target.gameObject.GetComponents<TGSJuiceBase>())
            {
                RenderJuiceEditor(juice, target);
            }

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            HandleAddJuicePopup(target);
            RenderPlayAllButton(target);
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private void FindJuiceTypes()
        {
            _juiceTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                           from assemblyType in domainAssembly.GetTypes()
                           where assemblyType.IsSubclassOf(typeof(TGSJuiceBase))
                           select assemblyType).ToList();
        }

        private void HandleAddJuicePopup(TGSJuices target)
        {
            Dictionary<string, List<string>> categorizedOptions = new Dictionary<string, List<string>>();
            foreach (var type in _juiceTypes)
            {
                JuiceLabelAttribute juiceLabelAttr = Attribute.GetCustomAttribute(type, typeof(JuiceLabelAttribute)) as JuiceLabelAttribute;
                string juiceLabel = juiceLabelAttr != null ? juiceLabelAttr.Label : type.Name;
                string category = juiceLabel.Split('/')[0];
                if (!categorizedOptions.ContainsKey(category))
                {
                    categorizedOptions[category] = new List<string>();
                }
                categorizedOptions[category].Add(juiceLabel);
            }

            List<string> options = new List<string>() { "Add new juice..." };
            foreach (var category in categorizedOptions.Keys)
            {
                foreach (var juice in categorizedOptions[category])
                {
                    options.Add(juice);
                }
            }
            int selectedJuiceType = TGSJuicesEditorStyling.DrawStyledPopup(0, options.ToArray(), TGSJuicesEditorStyling.PopupStyle);

            if (selectedJuiceType > 0)
            {
                Type selectedType = _juiceTypes[selectedJuiceType - 1];
                var newJuice = Undo.AddComponent(target.gameObject, selectedType) as TGSJuiceBase;
                newJuice.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
                int instanceID = newJuice.GetInstanceID();

                Undo.RecordObject(target, "Add Juice");
                target.juices.Add(newJuice);

                if (!_juiceEditors.ContainsKey(instanceID))
                {
                    Editor _newJuiceEditor = null;
                    CreateCachedEditor(newJuice, null, ref _newJuiceEditor);
                    _juiceEditors[instanceID] = _newJuiceEditor;
                }
            }
        }

        private void RenderPlayAllButton(TGSJuices target)
        {
            foreach (var item in target.juices)
            {
                item.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            }
            TGSJuicesEditorStyling.DrawStyledButton("Play All", TGSJuicesEditorStyling.ButtonStyle, () => target.PlayAll());
        }

        private void RenderJuiceEditor(TGSJuiceBase juice, TGSJuices target)
        {
            int instanceID = juice.GetInstanceID();

            Type type = juice.GetType();

            // If the editor for this type doesn't exist, create it
            if (!_juiceEditors.ContainsKey(instanceID))
            {
                Editor editor = null;
                CreateCachedEditor(juice, null, ref editor);
                _juiceEditors[instanceID] = editor;
            }

            // If the foldout state for this type doesn't exist, initialize it to 'false'
            if (!_juiceFoldouts.ContainsKey(instanceID))
            {
                _juiceFoldouts[instanceID] = false;
            }

            JuiceLabelAttribute juiceLabelAttr = Attribute.GetCustomAttribute(type, typeof(JuiceLabelAttribute)) as JuiceLabelAttribute;

            string label = type.Name;
            if (juiceLabelAttr != null)
            {
                string[] labelParts = juiceLabelAttr.Label.Split('/');
                label = labelParts.Length > 1 ? labelParts[1] : labelParts[0];
            }

            _juiceFoldouts[instanceID] = TGSJuicesEditorStyling.DrawStyledFoldout(_juiceFoldouts[instanceID], label, TGSJuicesEditorStyling.FoldoutStyle);

            // Create the buttons
            if (_juiceFoldouts[instanceID])
            {
                JuiceDescriptionAttribute juiceDesc = Attribute.GetCustomAttribute(type, typeof(JuiceDescriptionAttribute)) as JuiceDescriptionAttribute;
                if (juiceDesc != null)
                {
                    EditorGUILayout.LabelField(juiceDesc.Description, TGSJuicesEditorStyling.InfoStyle);
                }

                EditorGUI.indentLevel++;
                // Only call the custom editor's OnInspectorGUI
                _juiceEditors[instanceID].OnInspectorGUI();
                EditorGUI.indentLevel--;
                EditorGUILayout.BeginHorizontal();
                TGSJuicesEditorStyling.DrawStyledButton("Play", TGSJuicesEditorStyling.ButtonStyle, () => juice.Play());
                TGSJuicesEditorStyling.DrawStyledButton("Remove", TGSJuicesEditorStyling.ButtonStyle, () =>
                {
                    Undo.RecordObject(target, "Remove Juice");
                    target.juices.Remove(juice);
                    Undo.DestroyObjectImmediate(juice);
                });
                TGSJuicesEditorStyling.DrawStyledButton("Listners", TGSJuicesEditorStyling.PopupStyle, () =>
                    StringListSearchWindow.OpenSearchWindow(juice.ActionType));

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
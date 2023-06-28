using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TGSJuice
{
    [CustomEditor(typeof(TGSJuices))]
    public class TGSJuicesEditor : Editor
    {
        private List<Type> _juiceTypes;
        private Dictionary<Type, Editor> _juiceEditors = new Dictionary<Type, Editor>();
        private Dictionary<Type, bool> _juiceFoldouts = new Dictionary<Type, bool>();

        private void OnEnable()
        {
            FindJuiceTypes();
        }

        private void FindJuiceTypes()
        {
            _juiceTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                           from assemblyType in domainAssembly.GetTypes()
                           where assemblyType.IsSubclassOf(typeof(TGSJuiceBase))
                           select assemblyType).ToList();
        }

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

                Undo.RecordObject(target, "Add Juice");
                target.juices.Add(newJuice);

                if (!_juiceEditors.ContainsKey(selectedType))
                {
                    Editor _newJuiceEditor = null;
                    CreateCachedEditor(newJuice, null, ref _newJuiceEditor);
                    _juiceEditors[selectedType] = _newJuiceEditor;
                }
            }
        }

        private void RenderJuiceEditor(TGSJuiceBase juice, TGSJuices target)
        {
            Type type = juice.GetType();

            // If the editor for this type doesn't exist, create it
            if (!_juiceEditors.ContainsKey(type))
            {
                Editor editor = null;
                CreateCachedEditor(juice, null, ref editor);
                _juiceEditors[type] = editor;
            }

            // If the foldout state for this type doesn't exist, initialize it to 'false'
            if (!_juiceFoldouts.ContainsKey(type))
            {
                _juiceFoldouts[type] = false;
            }

            JuiceLabelAttribute juiceLabelAttr = Attribute.GetCustomAttribute(type, typeof(JuiceLabelAttribute)) as JuiceLabelAttribute;

            string label = type.Name;
            if (juiceLabelAttr != null)
            {
                string[] labelParts = juiceLabelAttr.Label.Split('/');
                label = labelParts.Length > 1 ? labelParts[1] : labelParts[0];
            }

            _juiceFoldouts[type] = TGSJuicesEditorStyling.DrawStyledFoldout(_juiceFoldouts[type], label, TGSJuicesEditorStyling.FoldoutStyle);

            // Create the buttons for 'Play' and 'Remove'
            if (_juiceFoldouts[type])
            {
                JuiceDescriptionAttribute juiceDesc = Attribute.GetCustomAttribute(type, typeof(JuiceDescriptionAttribute)) as JuiceDescriptionAttribute;
                if (juiceDesc != null)
                    EditorGUILayout.HelpBox(juiceDesc.Description, MessageType.Info);

                EditorGUI.indentLevel++;
                // Only call the custom editor's OnInspectorGUI
                _juiceEditors[type].OnInspectorGUI();
                EditorGUI.indentLevel--;
                EditorGUILayout.BeginHorizontal();
                TGSJuicesEditorStyling.DrawStyledButton("Play", TGSJuicesEditorStyling.ButtonStyle, () => juice.Play());
                TGSJuicesEditorStyling.DrawStyledButton("Remove", TGSJuicesEditorStyling.ButtonStyle, () =>
                {
                    Undo.RecordObject(target, "Remove Juice");
                    target.juices.Remove(juice);
                    Undo.DestroyObjectImmediate(juice);
                });
                EditorGUILayout.EndHorizontal();
            }
        }

        private void RenderPlayAllButton(TGSJuices target)
        {
            TGSJuicesEditorStyling.DrawStyledButton("Play All", TGSJuicesEditorStyling.ButtonStyle, () => target.PlayAll());
        }
    }
}
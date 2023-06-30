using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TGSJuice
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TGSJuices))]
    public class TGSJuicesEditor : Editor
    {
        private List<Type> _juiceTypes;
        private static Dictionary<int, Editor> _juiceEditors = new Dictionary<int, Editor>();
        private List<string> _popupOptions = new List<string>() { "Add new juice..." };
        private const string FoldOutStateKeyPrefix = "TGSJuice_FoldOut_";

        private void OnEnable()
        {
            FindJuiceTypes();
            InitalizePopupData();
        }

        private void FindJuiceTypes()
        {
            _juiceTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                           from assemblyType in domainAssembly.GetTypes()
                           where assemblyType.IsSubclassOf(typeof(TGSJuiceBase))
                           select assemblyType).ToList();
        }

        private void InitalizePopupData()
        {
            Dictionary<string, List<string>> categorizedOptions = new Dictionary<string, List<string>>();
            foreach (var type in _juiceTypes)
            {
                JuiceLabelAttribute juiceLabelAttr = Attribute.GetCustomAttribute(type, typeof(JuiceLabelAttribute)) as JuiceLabelAttribute;
                string juiceLabel = juiceLabelAttr?.Label ?? type.Name;
                string category = juiceLabel.Split('/')[0];
                if (!categorizedOptions.TryGetValue(category, out List<string> categoryOptions))
                {
                    categoryOptions = new List<string>();
                    categorizedOptions[category] = categoryOptions;
                }
                categoryOptions.Add(juiceLabel);
            }

            _popupOptions.AddRange(categorizedOptions.SelectMany(category => category.Value));
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

        #region RenderJuiceEditor
        private void RenderJuiceEditor(TGSJuiceBase juice, TGSJuices target)
        {
            int instanceID = juice.GetInstanceID();
            Type type = juice.GetType();

            CreateEditorIfNeeded(juice, instanceID);

            DrawFoldoutLabel(type, instanceID);

            if (EditorPrefs.GetBool(FoldOutStateKeyPrefix + instanceID))
            {
                DrawDescription(type);
                DrawFields(instanceID);
                GUILayout.BeginHorizontal();
                DrawPlayButton(juice);
                DrawRemoveButton(juice, target);
                DrawListenersButton(juice);
                GUILayout.EndHorizontal();
            }
        }

        private void CreateEditorIfNeeded(TGSJuiceBase juice, int instanceID)
        {
            if (!_juiceEditors.TryGetValue(instanceID, out Editor editor))
            {
                CreateCachedEditor(juice, null, ref editor);
                _juiceEditors[instanceID] = editor;
            }
        }

        private void DrawFoldoutLabel(Type type, int instanceID)
        {
            JuiceLabelAttribute juiceLabelAttr = Attribute.GetCustomAttribute(type, typeof(JuiceLabelAttribute)) as JuiceLabelAttribute;
            string label = type.Name;
            if (juiceLabelAttr != null)
            {
                string[] labelParts = juiceLabelAttr.Label.Split('/');
                label = labelParts.Length > 1 ? labelParts[1] : labelParts[0];
            }

            var foldoutState = EditorPrefs.GetBool(FoldOutStateKeyPrefix + instanceID);
            foldoutState = TGSJuicesEditorStyling.DrawStyledFoldout(foldoutState, label);
            EditorPrefs.SetBool(FoldOutStateKeyPrefix + instanceID, foldoutState);
        }

        private void DrawDescription(Type type)
        {
            JuiceDescriptionAttribute juiceDesc = Attribute.GetCustomAttribute(type, typeof(JuiceDescriptionAttribute)) as JuiceDescriptionAttribute;
            if (juiceDesc != null)
                TGSJuicesEditorStyling.DrawStyledDescription(juiceDesc.Description);
        }

        private void DrawFields(int instanceID)
        {
            EditorGUI.indentLevel++;
            _juiceEditors[instanceID].OnInspectorGUI();
            EditorGUI.indentLevel--;
        }

        private void DrawPlayButton(TGSJuiceBase juice)
        {
            TGSJuicesEditorStyling.DrawStyledButton("Play", () => juice.Play());
        }

        private void DrawRemoveButton(TGSJuiceBase juice, TGSJuices target)
        {
            TGSJuicesEditorStyling.DrawStyledButton("Remove", () =>
            {
                Undo.RecordObject(target, "Remove Juice");
                target.juices.Remove(juice);
                Undo.DestroyObjectImmediate(juice);
            });
        }

        private void DrawListenersButton(TGSJuiceBase juice)
        {
            TGSJuicesEditorStyling.DrawStyledButton("Listners", () =>
                StringListSearchWindow.OpenSearchWindow(juice.ActionType));
        }
        #endregion

        private void HandleAddJuicePopup(TGSJuices target)
        {
            int selectedJuiceType = TGSJuicesEditorStyling.DrawStyledPopup(0, _popupOptions.ToArray());

            if (selectedJuiceType > 0)
            {
                Type selectedType = _juiceTypes[selectedJuiceType - 1];
                var newJuice = Undo.AddComponent(target.gameObject, selectedType) as TGSJuiceBase;
                newJuice.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
                int instanceID = newJuice.GetInstanceID();

                Undo.RecordObject(target, "Add Juice");
                target.juices.Add(newJuice);

                if (!_juiceEditors.TryGetValue(instanceID, out Editor editor))
                {
                    CreateCachedEditor(newJuice, null, ref editor);
                    _juiceEditors[instanceID] = editor;
                }
            }
        }

        private void RenderPlayAllButton(TGSJuices target)
        {
            foreach (var item in target.juices)
            {
                item.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            }
            TGSJuicesEditorStyling.DrawStyledButton("Play All", () => target.PlayAll());
        }
    }
}
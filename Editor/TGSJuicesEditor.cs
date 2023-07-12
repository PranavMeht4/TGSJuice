using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TGSJuice
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TGSJuices))]
    public class TGSJuicesEditor : Editor
    {
        private Dictionary<int, Editor> _juiceEditors = new Dictionary<int, Editor>();
        private const string FoldOutStateKeyPrefix = "TGSJuice_FoldOut_";
        private TGSJuices targetTGSJuices;
        SerializedProperty delayProp;

        private void OnEnable()
        {
            delayProp = serializedObject.FindProperty("Delay");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(delayProp);

            targetTGSJuices = (TGSJuices)base.target;

            foreach (var juice in targetTGSJuices.GetComponents<TGSJuiceBase>())
            {
                RenderJuiceEditor(juice);
            }

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            HandleAddJuicePopup();
            RenderPlayAllButton();
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        #region RenderJuiceEditor
        private void RenderJuiceEditor(TGSJuiceBase juice)
        {
            int instanceID = juice.GetInstanceID();
            Type type = juice.GetType();

            CreateEditorIfNeeded(juice, instanceID);

            GUILayout.BeginVertical(TGSJuicesEditorStyling.BackgroundStyle);
            {
                DrawFoldoutLabel(type, instanceID, targetTGSJuices, juice);

                if (!_juiceEditors.ContainsKey(instanceID))
                {
                    GUILayout.EndVertical();
                    return;
                }
                if (EditorPrefs.GetBool(FoldOutStateKeyPrefix + instanceID))
                {
                    DrawDescription(type);
                    DrawFields(instanceID);
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    {
                        DrawPlayButton(juice);
                        DrawListenersButton(juice);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }

        private void CreateEditorIfNeeded(TGSJuiceBase juice, int instanceID)
        {
            if (!_juiceEditors.TryGetValue(instanceID, out Editor editor))
            {
                CreateCachedEditor(juice, null, ref editor);
                _juiceEditors[instanceID] = editor;
            }
        }

        private void DrawFoldoutLabel(Type type, int instanceID, TGSJuices target, TGSJuiceBase juice)
        {
            JuiceLabelAttribute juiceLabelAttr = Attribute.GetCustomAttribute(type, typeof(JuiceLabelAttribute)) as JuiceLabelAttribute;
            string label = type.Name;
            if (juiceLabelAttr != null)
            {
                string[] labelParts = juiceLabelAttr.Label.Split('/');
                label = labelParts.Length > 1 ? labelParts[1] : labelParts[0];
            }

            var foldoutState = EditorPrefs.GetBool(FoldOutStateKeyPrefix + instanceID);
            foldoutState = TGSJuicesEditorStyling.DrawStyledFoldout(foldoutState, label, juiceLabelAttr.Icon, () => RemoveAction(target, juice));
            EditorPrefs.SetBool(FoldOutStateKeyPrefix + instanceID, foldoutState);
        }

        private void RemoveAction(TGSJuices target, TGSJuiceBase juice)
        {
            Undo.RecordObject(target, "Remove Juice");
            _juiceEditors.Remove(juice.GetInstanceID());
            target.juices.Remove(juice);
            Undo.DestroyObjectImmediate(juice);
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

        private void DrawListenersButton(TGSJuiceBase juice)
        {
            TGSJuicesEditorStyling.DrawStyledButton("Listners", () =>
                ListnerWindow.ShowWindow(juice.ActionType));
        }
        #endregion

        private void HandleAddJuicePopup()
        {
            TGSJuicesEditorStyling.DrawStyledSearchWindow((Type type) => AddSelectedJuice(type));
        }

        private void AddSelectedJuice(Type type)
        {
            var newJuice = Undo.AddComponent(targetTGSJuices.gameObject, type) as TGSJuiceBase;

            newJuice.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            int instanceID = newJuice.GetInstanceID();

            Undo.RecordObject(target, "Add Juice");
            targetTGSJuices.juices.Add(newJuice);

            if (!_juiceEditors.TryGetValue(instanceID, out Editor editor))
            {
                CreateCachedEditor(newJuice, null, ref editor);
                _juiceEditors[instanceID] = editor;
            }
        }

        private void RenderPlayAllButton()
        {
            foreach (var item in targetTGSJuices.juices)
            {
                item.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            }
            TGSJuicesEditorStyling.DrawStyledButton("Play All", () => targetTGSJuices.PlayAll());
        }
    }
}
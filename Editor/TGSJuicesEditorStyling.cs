using UnityEditor;
using UnityEngine;

namespace TGSJuice
{
    public class TGSJuicesEditorStyling
    {
        // Define a style for button
        public static GUIStyle ButtonStyle
        {
            get
            {
                var style = new GUIStyle(GUI.skin.button);
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.MiddleCenter;
                return style;
            }
        }

        // Define a style for foldout
        public static GUIStyle FoldoutStyle
        {
            get
            {
                var style = new GUIStyle(EditorStyles.foldout);
                style.fontSize = 14;
                style.fontStyle = FontStyle.BoldAndItalic;
                return style;
            }
        }

        // Define a style for popup
        public static GUIStyle PopupStyle
        {
            get
            {
                var style = new GUIStyle(EditorStyles.popup);
                style.fontSize = 12;
                style.alignment = TextAnchor.MiddleLeft;
                return style;
            }
        }

        // GUI Drawing Methods
        public static void DrawStyledButton(string label, GUIStyle style, System.Action onClick)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(label, style, GUILayout.Width(100), GUILayout.Height(20), GUILayout.MaxWidth(75)))
            {
                onClick?.Invoke();
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        public static bool DrawStyledFoldout(bool foldout, string label, GUIStyle style)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            foldout = EditorGUILayout.Foldout(foldout, label, true, style);
            EditorGUILayout.EndVertical();
            return foldout;
        }

        public static int DrawStyledPopup(int selectedIndex, string[] options, GUIStyle style)
        {
            return EditorGUILayout.Popup(selectedIndex, options, style);
        }
    }
}
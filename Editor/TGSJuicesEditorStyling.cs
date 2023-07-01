using System;
using UnityEditor;
using UnityEngine;

namespace TGSJuice
{
    public static class TGSJuicesEditorStyling
    {
        // GUI style proprities

        private static GUIStyle InfoStyle
        {
            get
            {
                GUIStyle style = new GUIStyle(GUI.skin.label);
                style.richText = true;
                style.wordWrap = true;
                style.normal.textColor = Color.white;
                style.normal.background = MakeTex(1, 1, new Color(0.5f, 0.5f, 0.5f, .3f));
                style.padding = new RectOffset(5, 5, 3, 3);
                style.margin = new RectOffset(8, 8, 8, 8);
                return style;
            }
        }

        private static GUIStyle ButtonStyle
        {
            get
            {
                var style = new GUIStyle(GUI.skin.button);
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.MiddleCenter;
                return style;
            }
        }

        private static GUIStyle FoldoutStyle
        {
            get
            {
                var style = new GUIStyle(EditorStyles.foldout);
                style.font = EditorStyles.boldFont;
                style.alignment = TextAnchor.MiddleLeft;
                style.fontSize = 13;
                style.fontStyle = FontStyle.Bold;
                return style;
            }
        }

        private static GUIStyle PopupStyle
        {
            get
            {
                var style = new GUIStyle(EditorStyles.popup);
                style.fontSize = 13;
                style.alignment = TextAnchor.MiddleCenter;
                return style;
            }
        }

        public static GUIStyle BackgroundStyle
        {
            get
            {
                var style = new GUIStyle(GUI.skin.box);
                style.normal.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, .25f));
                return style;
            }
        }


        // GUI drawing methods

        internal static void DrawStyledDescription(string description)
        {
            EditorGUILayout.LabelField(description, InfoStyle);
        }

        public static void DrawStyledButton(string label, Action onClick)
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                {
                    if (GUILayout.Button(label, ButtonStyle, GUILayout.Width(100), GUILayout.Height(20)))
                        onClick?.Invoke();
                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
        }

        public static bool DrawStyledFoldout(bool foldout, string label, string icon, Action onClick)
        {
            GUIStyle backgroundStyle = new GUIStyle(GUI.skin.box);
            backgroundStyle.normal.background = MakeTex(1, 1, new Color(0.1f, 0.1f, 0.1f, .4f));

            EditorGUI.indentLevel++;
            GUILayout.BeginVertical(backgroundStyle);
            {
                GUILayout.BeginHorizontal();
                {
                    GUIContent content = new GUIContent($"   {label}", EditorGUIUtility.IconContent(icon).image);
                    foldout = EditorGUILayout.Foldout(foldout, content, true, FoldoutStyle);
                    if (GUILayout.RepeatButton("X", GUILayout.MaxWidth(20)))
                    {
                        onClick?.Invoke();
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            EditorGUI.indentLevel--;

            return foldout;
        }

        public static int DrawStyledPopup(int selectedIndex, string[] options)
        {
            return EditorGUILayout.Popup(selectedIndex, options, PopupStyle);
        }

        private static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
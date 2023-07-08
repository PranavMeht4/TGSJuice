using System;
using UnityEditor;
using UnityEngine;

namespace TGSJuice
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class JuiceLabelAttribute : Attribute
    {
        public string Label { get; private set; }
        public string Icon { get; private set; }

        public JuiceLabelAttribute(string label, string icon)
        {
            Label = label;
            Icon = icon;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class JuiceDescriptionAttribute : Attribute
    {
        public string Description { get; private set; }

        public JuiceDescriptionAttribute(string discription)
        {
            Description = discription;
        }
    }


    /// <summary>
    /// This attribute hides the field in the Unity inspector when the specified boolean property is true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class HideIfTrueAttribute : PropertyAttribute
    {
        public string ConditionPropertyName { get; private set; }

        public HideIfTrueAttribute(string conditionPropertyName)
        {
            ConditionPropertyName = conditionPropertyName;
        }
    }

    [CustomPropertyDrawer(typeof(HideIfTrueAttribute))]
    public class HideIfTruePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            HideIfTrueAttribute customAttribute = (HideIfTrueAttribute)attribute;
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(customAttribute.ConditionPropertyName);

            if (conditionProperty == null || !conditionProperty.boolValue)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            HideIfTrueAttribute customAttribute = (HideIfTrueAttribute)attribute;
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(customAttribute.ConditionPropertyName);

            if (conditionProperty == null || !conditionProperty.boolValue)
            {
                return base.GetPropertyHeight(property, label);
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// This attribute hides the field in the Unity inspector when the specified boolean property is false.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class HideIfFalseAttribute : PropertyAttribute
    {
        public string ConditionPropertyName { get; private set; }

        public HideIfFalseAttribute(string conditionPropertyName)
        {
            ConditionPropertyName = conditionPropertyName;
        }
    }

    [CustomPropertyDrawer(typeof(HideIfFalseAttribute))]
    public class HideIfFalsePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            HideIfFalseAttribute customAttribute = (HideIfFalseAttribute)attribute;
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(customAttribute.ConditionPropertyName);

            if (conditionProperty == null || conditionProperty.boolValue)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            HideIfFalseAttribute customAttribute = (HideIfFalseAttribute)attribute;
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(customAttribute.ConditionPropertyName);

            if (conditionProperty == null || conditionProperty.boolValue)
            {
                return base.GetPropertyHeight(property, label);
            }
            else
            {
                return 0;
            }
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace MochaLib.Editor
{
    public class RangeEditor : PropertyDrawer
    {
        private static readonly GUIContent MinLabel = new("Min");
        private static readonly GUIContent MaxLabel = new("Max");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var minProperty = property.FindPropertyRelative("minValue");
            var maxProperty = property.FindPropertyRelative("maxValue");

            ApplyValue(minProperty, maxProperty);

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                label = EditorGUI.BeginProperty(position, label, property);
                var fieldRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
                EditorGUI.indentLevel       = 0;
                EditorGUIUtility.labelWidth = 30f;

                // 最小値の描画
                var minRect = fieldRect;
                minRect.width /= 2f;
                EditorGUI.PropertyField(minRect, minProperty, MinLabel);

                // 最大値の描画
                var maxRect = fieldRect;
                maxRect.x     += minRect.width;
                maxRect.width =  minRect.width;
                EditorGUI.PropertyField(maxRect, maxProperty, MaxLabel);

                EditorGUI.EndProperty();
            }
        }

        protected virtual void ApplyValue(SerializedProperty minProperty, SerializedProperty maxProperty)
        {
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
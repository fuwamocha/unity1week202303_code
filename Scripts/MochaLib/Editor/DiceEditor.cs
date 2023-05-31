using MochaLib.Cores;
using UnityEditor;
using UnityEngine;

namespace MochaLib.Editor
{
    [CustomPropertyDrawer(typeof(MochaDice))]
    public class DiceEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var countProperty = property.FindPropertyRelative("diceCount");
            var valueProperty = property.FindPropertyRelative("sidedValue");

            // Prefabでプロパティを編集した際に太字になるようにする
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                label = EditorGUI.BeginProperty(position, label, property);
                var fieldRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
                EditorGUI.indentLevel       = 0;
                EditorGUIUtility.labelWidth = 30f;

                // ダイス数の描画
                var diceCountRect = fieldRect;
                diceCountRect.width /= 4.0f;
                EditorGUI.PropertyField(diceCountRect, countProperty, GUIContent.none);

                // dの描画
                var dRect = fieldRect;
                dRect.x     += diceCountRect.width;
                dRect.width =  18;
                EditorGUI.LabelField(dRect, " d ");

                // サイコロの面の描画
                var sidedValueRect = fieldRect;
                sidedValueRect.x     += diceCountRect.width + dRect.width;
                sidedValueRect.width =  diceCountRect.width;
                EditorGUI.PropertyField(sidedValueRect, valueProperty, GUIContent.none);

                EditorGUI.EndProperty();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
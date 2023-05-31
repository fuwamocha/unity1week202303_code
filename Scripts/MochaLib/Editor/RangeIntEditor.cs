using MochaLib.Cores;
using UnityEditor;

namespace MochaLib.Editor
{
    [CustomPropertyDrawer(typeof(MochaIntRange))]
    public class RangeIntEditor : RangeEditor
    {
        protected override void ApplyValue(SerializedProperty minProperty, SerializedProperty maxProperty)
        {
            if (maxProperty.intValue < minProperty.intValue) maxProperty.intValue = minProperty.intValue;
        }
    }
}
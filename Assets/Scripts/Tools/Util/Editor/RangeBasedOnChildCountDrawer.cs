using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RangeBasedOnChildCount))]
public class RangeBasedOnChildCountDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        var att = attribute as RangeBasedOnChildCount;
        position = EditorGUI.PrefixLabel(position, label);
        property.intValue=EditorGUI.IntSlider(position,property.intValue+1,1,
            ((MonoBehaviour)property.serializedObject.targetObject).transform.childCount-att.MaxOffset)-1;
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}

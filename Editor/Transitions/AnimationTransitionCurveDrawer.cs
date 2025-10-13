using UnityEngine;
using UnityEditor;

using Tellory.StackableUI.Transitions;

[CustomPropertyDrawer(typeof(AnimationTransitionCurve))]
public class AnimationTransitionCurveDrawer : PropertyDrawer
{
    // Methods
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float durationWidth = 60f;
        float spacing = 5f;

        Rect durationRect = new Rect(position.x, position.y, durationWidth, position.height);
        Rect curveRect = new Rect(position.x + durationWidth + spacing, position.y, position.width - durationWidth - spacing, position.height);

        SerializedProperty durationProp = property.FindPropertyRelative("m_duration");
        SerializedProperty curveProp = property.FindPropertyRelative("m_animationCurve");

        EditorGUI.PropertyField(durationRect, durationProp, GUIContent.none);
        EditorGUI.PropertyField(curveRect, curveProp, GUIContent.none);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}

using UnityEngine;
using UnityEditor;

using Tellory.StackableUI.Transitions;

[CustomEditor(typeof(PositionTransition))]
[CanEditMultipleObjects]
public class PositionTransitionEditor : TransitionBaseEditor
{
    // Fields
    private SerializedProperty m_rectTransform;
    private SerializedProperty m_transitionDirection;
    private SerializedProperty m_movementStrength;

    // Methods
    protected override void OnEnable()
    {
        base.OnEnable();

        m_rectTransform = serializedObject.FindProperty("m_rectTransform");
        m_transitionDirection = serializedObject.FindProperty("m_transitionDirection");
        m_movementStrength = serializedObject.FindProperty("m_movementStrength");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        PrettyHeader.DrawLabel("Position", PrettyHeader.Green);

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(m_rectTransform, new GUIContent("Rect Transform"));
        EditorGUILayout.PropertyField(m_transitionDirection, new GUIContent("Transition Direction"));
        EditorGUILayout.PropertyField(m_movementStrength, new GUIContent("Movement Strength"));

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}

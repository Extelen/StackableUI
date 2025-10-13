using UnityEngine;
using UnityEditor;

using Tellory.StackableUI.Transitions;

[CustomEditor(typeof(OpacityTransition))]
[CanEditMultipleObjects]
public class OpacityTransitionEditor : TransitionBaseEditor
{
    // Fields
    private SerializedProperty m_canvasGroup;
    private SerializedProperty m_controlInteractivity;

    // Methods
    protected override void OnEnable()
    {
        base.OnEnable();

        m_canvasGroup = serializedObject.FindProperty("m_canvasGroup");
        m_controlInteractivity = serializedObject.FindProperty("m_controlInteractivity");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        PrettyHeader.DrawLabel("Opacity", PrettyHeader.Green);

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(m_canvasGroup, new GUIContent("Canvas Group"));
        EditorGUILayout.PropertyField(m_controlInteractivity, new GUIContent("Control Interactivity"));

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}

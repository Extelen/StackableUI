using UnityEngine;
using UnityEditor;

using Tellory.StackableUI.Transitions;

public class TransitionBaseEditor : Editor
{
    // Fields
    private SerializedProperty m_showProperty;
    private SerializedProperty m_hideProperty;

    // Methods
    protected virtual void OnEnable()
    {
        m_showProperty = serializedObject.FindProperty("m_showAnimation");
        m_hideProperty = serializedObject.FindProperty("m_hideAnimation");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        PrettyHeader.DrawLabel("Animation", PrettyHeader.Green);

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(m_showProperty, new GUIContent("Show"));
        EditorGUILayout.PropertyField(m_hideProperty, new GUIContent("Hide"));

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}

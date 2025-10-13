using UnityEngine;
using UnityEditor;

using Tellory.StackableUI.Views;

[CustomEditor(typeof(StackableViewGroup))]
[CanEditMultipleObjects]
public class StackableViewGroupEditor : StackableViewBaseEditor
{
    // Fields
    private SerializedProperty m_viewBehavioursProperty;

    // Methods
    protected override void OnEnable()
    {
        base.OnEnable();
        m_viewBehavioursProperty = serializedObject.FindProperty("m_viewBehaviours");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(m_viewBehavioursProperty, new GUIContent("Sub Views"));

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}

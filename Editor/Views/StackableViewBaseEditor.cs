using UnityEngine;
using UnityEditor;

public class StackableViewBaseEditor : StackNesterBaseEditor
{
    // Fields
    private SerializedProperty m_identifierProperty;
    private SerializedProperty m_isBackNavigationAllowedProperty;

    // Methods
    protected override void OnEnable()
    {
        base.OnEnable();
        m_identifierProperty = serializedObject.FindProperty("m_identifier");
        m_isBackNavigationAllowedProperty = serializedObject.FindProperty("m_isBackNavigationAllowed");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUI.indentLevel++;

        PrettyHeader.DrawLabel("View Settings", PrettyHeader.Blue);
        EditorGUILayout.PropertyField(m_identifierProperty, new GUIContent("Identifier"));
        EditorGUILayout.PropertyField(m_isBackNavigationAllowedProperty, new GUIContent("Is Back Navigation Allowed?"));

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}

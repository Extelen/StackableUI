using UnityEngine;
using UnityEditor;

using Tellory.StackableUI.Navigation;

[CustomEditor(typeof(InputSystemBackNavigation))]
public class InputSystemBackNavigationEditor : BackNavigationBaseEditor
{
    // Fields
    private SerializedProperty m_backActionReferenceProperty;

    // Methods
    protected override void OnEnable()
    {
        base.OnEnable();
        m_backActionReferenceProperty = serializedObject.FindProperty("m_backActionReference");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        PrettyHeader.DrawLabel("Input System Settings", PrettyHeader.Red);

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(m_backActionReferenceProperty, new GUIContent("Back Input Action"));

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}

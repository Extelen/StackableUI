using UnityEngine;
using UnityEditor;

using Tellory.StackableUI.Navigation;

[CustomEditor(typeof(InputManagerBackNavigation))]
public class InputManagerBackNavigationEditor : BackNavigationBaseEditor
{
    // Fields
    private SerializedProperty m_keyCodeProperty;

    // Methods
    protected override void OnEnable()
    {
        base.OnEnable();
        m_keyCodeProperty = serializedObject.FindProperty("m_keyCode");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        PrettyHeader.DrawLabel("Input Manager Settings", PrettyHeader.Red);

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(m_keyCodeProperty, new GUIContent("Back Key"));

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}

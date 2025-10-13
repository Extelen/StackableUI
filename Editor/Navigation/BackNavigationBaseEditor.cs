using UnityEngine;
using UnityEditor;

using Tellory.StackableUI.Navigation;

public class BackNavigationBaseEditor : Editor
{
    // Fields
    private SerializedProperty m_backHandlerBehaviourProperty;

    // Methods
    protected virtual void OnEnable()
    {
        m_backHandlerBehaviourProperty = serializedObject.FindProperty("m_backHandlerBehaviour");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        PrettyHeader.DrawLabel("Back Navigation", PrettyHeader.Red);

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(m_backHandlerBehaviourProperty, new GUIContent("Back Handler"));
        TypeRequirement.DrawObjectRequiredImplementation<IViewBackHandler>(m_backHandlerBehaviourProperty);

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}

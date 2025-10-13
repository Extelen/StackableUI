using UnityEngine;
using UnityEditor;

using Tellory.StackableUI.Views;
using Tellory.StackableUI.Transitions;

[CustomEditor(typeof(StackableView))]
[CanEditMultipleObjects]
public class StackableViewEditor : StackableViewBaseEditor
{
    // Fields
    private SerializedProperty m_transitionBehaviourProperty;
    private SerializedProperty m_useScaledTimeProperty;

    // Methods
    protected override void OnEnable()
    {
        base.OnEnable();
        m_transitionBehaviourProperty = serializedObject.FindProperty("m_transitionBehaviour");
        m_useScaledTimeProperty = serializedObject.FindProperty("m_useScaledTime");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUI.indentLevel++;

        PrettyHeader.DrawLabel("Transitions", PrettyHeader.Green);

        EditorGUILayout.PropertyField(m_transitionBehaviourProperty, new GUIContent("Transitions"));
        TypeRequirement.DrawObjectRequiredImplementation<ITransition>(m_transitionBehaviourProperty);

        EditorGUILayout.PropertyField(m_useScaledTimeProperty, new GUIContent("Use Scaled Time"));

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}

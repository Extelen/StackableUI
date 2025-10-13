using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using Tellory.StackableUI.Views;

public class StackNesterBaseEditor : Editor
{
    // Fields
    private SerializedProperty m_showDefaultViewInstantlyProperty;
    private SerializedProperty m_defaultNestedViewProperty;
    private SerializedProperty m_openDefaultViewAtNestedCloseProperty;
    private SerializedProperty m_nestedViewBehavioursProperty;

    private List<string> m_requiredImplementationCacheNames;

    // Methods
    protected virtual void OnEnable()
    {
        m_showDefaultViewInstantlyProperty = serializedObject.FindProperty("m_showDefaultViewInstantly");
        m_defaultNestedViewProperty = serializedObject.FindProperty("m_defaultNestedView");
        m_openDefaultViewAtNestedCloseProperty = serializedObject.FindProperty("m_openDefaultViewAtNestedClose");
        m_nestedViewBehavioursProperty = serializedObject.FindProperty("m_nestedViewBehaviours");

        m_requiredImplementationCacheNames = new List<string>();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        PrettyHeader.DrawLabel("Nesting", PrettyHeader.Purple);

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(m_nestedViewBehavioursProperty, new GUIContent("Views"), true);
        TypeRequirement.DrawArrayRequiredImplementation<IView>(m_nestedViewBehavioursProperty, m_requiredImplementationCacheNames);

        if (m_nestedViewBehavioursProperty.arraySize > 0)
        {
            EditorGUILayout.PropertyField(m_defaultNestedViewProperty, new GUIContent("Default IView"));
            bool defaultViewIsCorrectType = TypeRequirement.DrawObjectRequiredImplementation<IView>(m_defaultNestedViewProperty);

            if (defaultViewIsCorrectType)
            {
                EditorGUILayout.PropertyField(m_showDefaultViewInstantlyProperty);
                EditorGUILayout.PropertyField(m_openDefaultViewAtNestedCloseProperty);
            }
        }

        EditorGUI.indentLevel--;
        serializedObject.ApplyModifiedProperties();
    }
}

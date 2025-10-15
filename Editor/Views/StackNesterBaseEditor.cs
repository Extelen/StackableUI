using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using Tellory.StackableUI.Views;

public class StackNesterBaseEditor : Editor
{
    // Fields
    private SerializedProperty m_showInitialViewInstantlyProperty;
    private SerializedProperty m_initialViewProperty;
    private SerializedProperty m_useInitialViewAsBaseProperty;
    private SerializedProperty m_baseViewProperty;
    private SerializedProperty m_nestedViewBehavioursProperty;

    private List<string> m_requiredImplementationCacheNames;

    // Methods
    protected virtual void OnEnable()
    {
        m_showInitialViewInstantlyProperty = serializedObject.FindProperty("m_showInitialViewInstantly");
        m_initialViewProperty = serializedObject.FindProperty("m_initialView");
        m_useInitialViewAsBaseProperty = serializedObject.FindProperty("m_useInitialViewAsBase");
        m_baseViewProperty = serializedObject.FindProperty("m_baseView");
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
            EditorGUILayout.PropertyField(m_initialViewProperty, new GUIContent("Initial IView"));
            bool initialViewIsIView = TypeRequirement.DrawObjectRequiredImplementation<IView>(m_initialViewProperty);

            if (initialViewIsIView)
            {
                EditorGUILayout.PropertyField(m_showInitialViewInstantlyProperty);
                EditorGUILayout.PropertyField(m_useInitialViewAsBaseProperty);

                if (m_useInitialViewAsBaseProperty.boolValue == false)
                {
                    EditorGUILayout.PropertyField(m_baseViewProperty, new GUIContent("Base IView"));
                    TypeRequirement.DrawObjectRequiredImplementation<IView>(m_baseViewProperty);
                }
            }
        }

        EditorGUI.indentLevel--;
        serializedObject.ApplyModifiedProperties();
    }
}

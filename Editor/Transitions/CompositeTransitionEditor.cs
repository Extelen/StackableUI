using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Tellory.StackableUI.Transitions;

[CustomEditor(typeof(CompositeTransition))]
public class CompositeTransitionEditor : Editor
{
    // Fields
    private SerializedProperty m_transitionBehaviours;
    private List<string> m_notITransitionViewNamesCache;

    // Methods
    protected void OnEnable()
    {
        m_notITransitionViewNamesCache = new List<string>();
        m_transitionBehaviours = serializedObject.FindProperty("m_transitionBehaviours");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        PrettyHeader.DrawLabel("Composite", PrettyHeader.Green);

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(m_transitionBehaviours, new GUIContent("Transition Behaviours"), true);

        bool hasNotIViewChilds = false;
        m_notITransitionViewNamesCache.Clear();

        for (int i = 0; i < m_transitionBehaviours.arraySize; i++)
        {
            var element = m_transitionBehaviours.GetArrayElementAtIndex(i);

            if (element == null)
                continue;

            if (element.objectReferenceValue == null)
                continue;

            if (element.objectReferenceValue is ITransition)
                continue;

            m_notITransitionViewNamesCache.Add((element.objectReferenceValue as MonoBehaviour).name);
            hasNotIViewChilds = true;
        }

        if (hasNotIViewChilds)
        {
            string names = "";

            for (int i = 0; i < m_notITransitionViewNamesCache.Count; i++)
            {
                names += m_notITransitionViewNamesCache[i];

                if (i == m_notITransitionViewNamesCache.Count - 2)
                    names += " and ";

                else if (i == m_notITransitionViewNamesCache.Count - 1)
                    names += " ";

                else
                    names += ", ";
            }

            EditorGUILayout.HelpBox(names + "doesn't implement ITransition", MessageType.Error);
        }

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}

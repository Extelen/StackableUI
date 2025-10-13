using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class TypeRequirement
{
    // Methods
    public static bool DrawObjectRequiredImplementation<T>(SerializedProperty property)
    {
        Object obj = property.objectReferenceValue;

        if (obj == null)
            return false;

        if (obj is T)
            return true;

        else
        {
            EditorGUILayout.HelpBox($"{property.name} doesn't implement {typeof(T)}", MessageType.Error);
            return false;
        }
    }

    public static void DrawArrayRequiredImplementation<T>(SerializedProperty arrayProperty, List<string> cache)
    {
        bool isImplementationRequiredInChild = false;
        cache.Clear();

        for (int i = 0; i < arrayProperty.arraySize; i++)
        {
            var element = arrayProperty.GetArrayElementAtIndex(i);

            if (element == null)
                continue;

            if (element.objectReferenceValue == null)
                continue;

            if (element.objectReferenceValue is T)
                continue;

            cache.Add((element.objectReferenceValue as MonoBehaviour).name);
            isImplementationRequiredInChild = true;
        }

        if (isImplementationRequiredInChild)
        {
            string names = "";

            for (int i = 0; i < cache.Count; i++)
            {
                names += cache[i];

                if (i == cache.Count - 2)
                    names += " and ";

                else if (i == cache.Count - 1)
                    names += " ";

                else
                    names += ", ";
            }

            EditorGUILayout.HelpBox(names + "doesn't implement " + nameof(T), MessageType.Error);
        }
    }
}

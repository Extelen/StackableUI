using UnityEngine;
using UnityEditor;

public static class PrettyHeader
{
    // Fields
    private static GUIStyle m_style = new GUIStyle
    {
        alignment = TextAnchor.MiddleLeft,
        padding = new(6, 6, 6, 6),
        fontSize = 14,
        fontStyle = FontStyle.Bold,
        normal = { textColor = Color.white },
    };

    // Methods
    public static void DrawLabel(string label, Color headerColor)
    {
        var indentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorGUILayout.Space(12);

        Rect rect = GUILayoutUtility.GetRect(0f, 24f, GUILayout.ExpandWidth(true));
        EditorGUI.DrawRect(rect, headerColor);
        GUI.Label(rect, label, m_style);

        EditorGUILayout.Space(4);

        EditorGUI.indentLevel = indentLevel;
    }

    public static Color Purple => new Color(0.35f, 0.25f, 0.35f, 0.6f);
    public static Color Red => new Color(0.35f, 0.20f, 0.20f, 0.6f);
    public static Color Green => new Color(0.20f, 0.35f, 0.20f, 0.6f);
    public static Color Orange => new Color(0.80f, 0.00f, 0.45f, 0.6f);
    public static Color Pink => new Color(0.45f, 0.00f, 0.55f, 0.6f);
    public static Color Blue => new Color(0.25f, 0.25f, 0.50f, 0.6f);
}

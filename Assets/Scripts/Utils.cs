using UnityEngine;
using System;
using System.Linq;

class Utils {
    public static Collider[] GetAllColliders(GameObject gameObject) {
        return gameObject.GetComponentsInChildren<Collider>();
    }

    public static Collider[] GetAllColliders(GameObject gameObject, bool trigger) {
        return GetAllColliders(gameObject).Where(x => x.isTrigger == trigger).ToArray();
    }

    public static Collider[] GetAllColliders() {
        return GameObject.FindObjectsOfType<Collider>();
    }

    public static Collider[] GetAllColliders(bool trigger) {
        return GetAllColliders().Where(x => x.isTrigger == trigger).ToArray(); 
    }

    public static Component CopyComponent(Component original)
    {
        System.Type type = original.GetType();
        Component copy = original.gameObject.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields(); 
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }

    public static float Clip(float x, float min, float max) {
        return Math.Max(min, Math.Min(max, x));
    }

    public static float Distance(Vector3 first, Vector3 second) {
        return Mathf.Sqrt(Mathf.Pow(first.x - second.x, 2) + Mathf.Pow(first.z - second.z, 2));
    }

    public static void DrawOutline(Rect rect, string text, GUIStyle style, Color outColor, Color inColor)
    {
        float halfSize = 1.0f;
        GUIStyle backupStyle = new GUIStyle(style);
        Color backupColor = GUI.color;
        style.alignment = TextAnchor.UpperCenter;

        style.normal.textColor = outColor;
        GUI.color = outColor;

        rect.x -= halfSize;
        GUI.Label(rect, text, style);

        rect.x += 2 * halfSize;
        GUI.Label(rect, text, style);

        rect.x -= halfSize;
        rect.y -= halfSize;
        GUI.Label(rect, text, style);

        rect.y += 2 * halfSize;
        GUI.Label(rect, text, style);

        rect.y -= halfSize;
        style.normal.textColor = inColor;
        GUI.color = backupColor;
        GUI.Label(rect, text, style);

        style = backupStyle;
    }

    public static bool AreTheseTwoFloatsNearlyTheSamePlease(float a, float b) {
        return Math.Abs(a - b) < 0.1;
    }
}
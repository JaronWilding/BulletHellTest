using UnityEditor;
using UnityEngine;

public class FoldoutUsage : EditorWindow
{
    bool showPosition = true;
    string status = "Select a GameObject";

    [MenuItem("Examples/Foldout Usage")]
    static void Init()
    {
        FoldoutUsage window = (FoldoutUsage)GetWindow(typeof(FoldoutUsage));
        window.Show();
    }

    public void OnGUI()
    {
        showPosition = EditorGUILayout.Foldout(showPosition, status);
        if (showPosition)
            if (Selection.activeTransform)
            {
                Selection.activeTransform.position =
                    EditorGUILayout.Vector3Field("Position", Selection.activeTransform.position);
                status = Selection.activeTransform.name;
            }

        if (!Selection.activeTransform)
        {
            status = "Select a GameObject";
            showPosition = false;
        }
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}
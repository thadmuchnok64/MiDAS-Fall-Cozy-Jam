using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphStyles : EditorWindow
{
    [SerializeField]
    private StyleSheet m_StyleSheet = default;

    [MenuItem("Window/UI Toolkit/DialogueGraphStyles")]
    public static void ShowExample()
    {
        DialogueGraphStyles wnd = GetWindow<DialogueGraphStyles>();
        wnd.titleContent = new GUIContent("DialogueGraphStyles");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Add label
        VisualElement labelWithStyle = new Label("Hello World! With Style");
        labelWithStyle.AddToClassList("custom-label");
        labelWithStyle.styleSheets.Add(m_StyleSheet);
        root.Add(labelWithStyle);
    }
}

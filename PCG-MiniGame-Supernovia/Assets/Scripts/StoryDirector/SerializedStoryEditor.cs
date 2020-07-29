using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SerializedStoryEditor : EditorWindow
{
    private SerializedStory editTarget = null;
    private DynamicWidgetGroup<SerializedStorySectionWidget, SerializedStory.Section> sectionWidgets;

    private Vector2 scrollView;

    [MenuItem("PCG编辑/测试/SerilizedStoryDirector")]
    public static void ShowWindow() {
        GetWindow(typeof(SerializedStoryEditor));
    }

    private void OnEnable() {
        var storyPlayer = FindObjectOfType<SerializedStoryPlayer>();
        if (storyPlayer != null) {
            editTarget = storyPlayer.serializedStory;
        }
        if (editTarget != null) {
            sectionWidgets = new DynamicWidgetGroup<SerializedStorySectionWidget, SerializedStory.Section>(editTarget.sections,false);
        }
    }

    private void OnGUI() {
        EditorGUILayout.HelpBox("这里编辑都是原card的副本，不是引用！", MessageType.Warning);
        scrollView = EditorGUILayout.BeginScrollView(scrollView);

        if (sectionWidgets != null) {
            if (GUILayout.Button("+")) {
                editTarget.sections.Add(new SerializedStory.Section());
            }
            sectionWidgets.RenderUI();
        }
        else {
            GUILayout.Label("Cannot find SerilizedStoryPlayer.serilizedStory in the scene");
        }

        EditorGUILayout.EndScrollView();
    }


    private void OnDisable() {
        EditorUtility.SetDirty(editTarget);
        AssetDatabase.Refresh();
    }
}

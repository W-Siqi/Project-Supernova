using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// 搜索修饰词库
public class SearchQualiferWindow : EditorWindow {
    public delegate void OnOneQulifierSelected(Qualifier selectedQualifer);

    private OnOneQulifierSelected onOneQulifierSelected;
    private QualifierLibrary targetLib;

    public static void  Create(QualifierLibrary targetLib, OnOneQulifierSelected onOneQulifierSelected) {
        var newWindow = CreateInstance<SearchQualiferWindow>();
        newWindow.onOneQulifierSelected = onOneQulifierSelected;
        newWindow.targetLib = targetLib;
        newWindow.Show();
    }

    private void OnGUI() {
        //EditorGUILayout.BeginHorizontal();
        //var allOptions = targetLib.
        //selectedIndex = EditorGUILayout.Popup(selectedIndex, allOptions, GUILayout.Width(EditorStyleResource.SEARCH_BAR_WIDTH));
        //if (GUILayout.Button("+", GUILayout.Width(BUTTON_WIDTH))) {
        //    // index 有效 
        //    if (selectedIndex < allOptions.Length) {
        //        var newItem = buildItem(allOptions[selectedIndex]);
        //        editTarget.Add(newItem);
        //    }
        //}
        //EditorGUILayout.EndHorizontal();
    }
}

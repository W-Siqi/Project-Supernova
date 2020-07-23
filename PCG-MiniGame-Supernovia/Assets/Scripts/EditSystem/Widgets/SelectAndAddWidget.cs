using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// 专门用来搜索添加对应的项目
public class SelectAndAddWidget<T>:Widget
{
    // 基于已添加的，获得当前还可添加的选项
    public delegate string[] GetPotencialOptions();
    public delegate T BuildItem(string selectedOption);

    private List<T> editTarget;
    private GetPotencialOptions getPotencialOptions;
    private BuildItem buildItem;
    private int selectedIndex = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="editTarget">要编辑的数组</param>
    /// <param name="getAllOptions">获得所有可选的对象</param>
    /// <param name="buildItem">通过选中的选项创建添加T的实例</param>
    public SelectAndAddWidget(List<T> editTarget, GetPotencialOptions getPotencialOptions, BuildItem buildItem) {
        this.editTarget = editTarget;
        this.getPotencialOptions = getPotencialOptions;
        this.buildItem = buildItem;
    }

    public override void RenderUI() {
        int BUTTON_WIDTH = 20;

        EditorGUILayout.BeginHorizontal();
        var allOptions = getPotencialOptions();
        selectedIndex = EditorGUILayout.Popup(selectedIndex, allOptions,GUILayout.Width(EditorStyleResource.SEARCH_BAR_WIDTH));
        if (GUILayout.Button("+", GUILayout.Width(BUTTON_WIDTH))) {
            // index 有效 
            if (selectedIndex < allOptions.Length) {
                var newItem = buildItem(allOptions[selectedIndex]);
                editTarget.Add(newItem);             
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// 所有card都可用的
/// </summary>
public class CardReferenceWidget : Widget
{
    private Card editTarget;

    public CardReferenceWidget(Card card) {
        editTarget = card;
    }

    public override void RenderUI() {
        int widgetWidth = 100;
        EditorGUILayout.BeginVertical();
        GUILayout.Label(editTarget.name,GUILayout.Width(widgetWidth));
        GUILayout.Label(editTarget.GetAvatarImage(), GUILayout.Width(widgetWidth), GUILayout.Height(widgetWidth));
        if (GUILayout.Button("查看", GUILayout.Width(widgetWidth))){
            CardEditor.ShowWindow(editTarget);
        }
        EditorGUILayout.EndHorizontal();
    }
}

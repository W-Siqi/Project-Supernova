using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 渲染对事件卡/决策卡 修饰词编辑的模块
public class DescriptionBlock{
    static string[] descriptionMaskArray= new string[] { "前置 - 人物", "前置 - 环境", "前置 - 事件", "后果 - 人物", "后果 - 环境" };
   
    private int mask = 0;
    private PreconditionWidget preconditionWidget;
    private ConsequenceWidget consequenceWidget;

    public DescriptionBlock(EventCard eventCard) {
        preconditionWidget = new PreconditionWidget(eventCard.preconditonSet);
        consequenceWidget = new ConsequenceWidget(eventCard.consequenceSet);
    }

    public DescriptionBlock(StratagemCard stratagemCard) {
        preconditionWidget = new PreconditionWidget(stratagemCard.preconditonSet);
        consequenceWidget = new ConsequenceWidget(stratagemCard.consequenceSet);
    }

    // PS: mask设置这里是硬编码
    public void RenderUI() {
        // render mask
        EditorGUILayout.BeginHorizontal();
        mask = EditorGUILayout.MaskField(mask, descriptionMaskArray);
        EditorGUILayout.EndHorizontal();

        // 根据mask设置修饰词的有效性以及编辑
        var selectedMask = new bool[5];
        for (int i = 0; i < selectedMask.Length; i++) {
            selectedMask[i] = (mask & (1 << i)) != 0;
        }
        // 前提描述绘制
        preconditionWidget.SetMask(selectedMask[0], selectedMask[1], selectedMask[2]);
        preconditionWidget.RenderUI();

        // 后果描述绘制
        consequenceWidget.SetMask(selectedMask[3],selectedMask[4]);
        consequenceWidget.RenderUI();
    }
}

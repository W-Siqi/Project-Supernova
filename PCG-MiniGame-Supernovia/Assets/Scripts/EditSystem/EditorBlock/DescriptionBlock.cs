using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PCG;

// 渲染对事件卡/决策卡两种卡的 修饰词编辑的模块
public class DescriptionBlock{
    const int DESCRIPTION_COUNT = 7;
    const string DOWN_ARRORW_IMAGE_PATH = "Assets/ArtResourse/UI/downarrow.png";

    static string[] descriptionMaskArray= new string[] { "前置 - 人物", "前置 - 环境", "前置 - 事件", "后果 - 人物", "后果 - 环境","后果 - 战斗","后果-状态值" };
   
    private int mask ;
    private PreconditionWidget preconditionWidget;
    private ConsequenceWidget consequenceWidget;

    public DescriptionBlock(EventCard eventCard) {
        preconditionWidget = new PreconditionWidget(eventCard.preconditonSet);
        consequenceWidget = new ConsequenceWidget(eventCard.consequenceSet);
        InitMask(eventCard.preconditonSet,eventCard.consequenceSet);
    }

    public DescriptionBlock(StratagemCard stratagemCard) {
        preconditionWidget = new PreconditionWidget(stratagemCard.preconditonSet);
        consequenceWidget = new ConsequenceWidget(stratagemCard.consequenceSet);
        InitMask(stratagemCard.preconditonSet, stratagemCard.consequenceSet);
    }

    private void InitMask(PreconditonSet preconditonSet, ConsequenceSet consequenceSet) {
        mask = 0;
        if (preconditonSet.characterEnabled) {
            mask |= (1 << 0);
        }
        if (preconditonSet.environmentEnabled) {
            mask |= (1 << 1);
        }
        if (preconditonSet.eventEnabled) {
            mask |= (1 << 2);
        }

        if (consequenceSet.characterConsequenceEnabled) {
            mask |= (1 << 3);
        }
        if (consequenceSet.environmentConsequenceEnabled) {
            mask |= (1 << 4);
        }
        if (consequenceSet.fightConsequenceEnabled) {
            mask |= (1 << 5);
        }
        if (consequenceSet.statusConsequenceEnabled) {
            mask |= (1 << 6);
        }
    }

    // PS: mask设置这里是硬编码
    public void RenderUI() {
        // render mask
        EditorGUILayout.BeginHorizontal();
        mask = EditorGUILayout.MaskField(mask, descriptionMaskArray);
        EditorGUILayout.EndHorizontal();

        // 根据mask设置修饰词的有效性以及编辑
        var selectedMask = new bool[DESCRIPTION_COUNT];
        for (int i = 0; i < selectedMask.Length; i++) {
            selectedMask[i] = (mask & (1 << i)) != 0;
        }
        // 前提描述绘制
        preconditionWidget.SetMask(selectedMask[0], selectedMask[1], selectedMask[2]);
        preconditionWidget.RenderUI();
        //GUILayout.Label(EditorStyleResource.LoadTex(DOWN_ARRORW_IMAGE_PATH),GUILayout.Height(100));
        // 后果描述绘制
        consequenceWidget.SetMask(selectedMask[3],selectedMask[4],selectedMask[5],selectedMask[6]);
        consequenceWidget.RenderUI();
    }
}

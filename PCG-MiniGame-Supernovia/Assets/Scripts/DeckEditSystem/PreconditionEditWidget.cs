using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PreconditionEditWidget {

    private PreconditonSet editTarget;

    // 每个绑定对象的绘制，引用一个QualifierEditWidget
    private Dictionary<CharacterPrecondition, QualifierEditWidget> charPrecontionWidgets;
    private QualifierEditWidget envPrecontionWidgets;
    private Dictionary<EventPrecondition, EventCard> eventPrectionsDict;

    private int eventPreconditionPopupIndex = 0;

    public PreconditionEditWidget(PreconditonSet target) {
        editTarget = target;

        // init character
        charPrecontionWidgets = new Dictionary<CharacterPrecondition, QualifierEditWidget>();
        foreach (var condition in target.characterPreconditions) {
            charPrecontionWidgets[condition] = new QualifierEditWidget(condition.qualifiers, DeckArchive.instance.characterQualifierLib);
        }

        // init enviroment
        envPrecontionWidgets = new QualifierEditWidget(target.environmentPrecondition.qualifiers, DeckArchive.instance.environmentQualifierLib);

        // init event
        eventPrectionsDict = new Dictionary<EventPrecondition, EventCard>();
        var invalidPreconditon = new List<EventPrecondition>();
        foreach (var eventPreconditon in editTarget.eventPreconditions) {
            foreach (var match in DeckArchive.instance.eventCards) {
                if (match.name == eventPreconditon.eventCardName) {
                    eventPrectionsDict[eventPreconditon] = match;
                }
            }
            
            if (!eventPrectionsDict.ContainsKey(eventPreconditon)) {
                invalidPreconditon.Add(eventPreconditon);
            }
        }
        // 清除无效的前置牌
        foreach (var condi in invalidPreconditon) {
            editTarget.eventPreconditions.Remove(condi);
        }
    }

    public void RenderUI() {
        RenderChadracterPrecontionUI();
        RenderEnvironmentPrecontionUI();
        RenderEventPreconditionUI();
    }

    private void RenderChadracterPrecontionUI() {
        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.LabelField("前置人物");
        // 添加新的人物前提
        if (GUILayout.Button("+")) {
            var newPrecondition = new CharacterPrecondition();
            editTarget.characterPreconditions.Add(newPrecondition);
            charPrecontionWidgets[newPrecondition] = new QualifierEditWidget(newPrecondition.qualifiers, DeckArchive.instance.characterQualifierLib);
        }

        EditorGUILayout.BeginVertical();
        var toDelete = new List<CharacterPrecondition>();
        // 逐个绘制人物前提编辑控件
        foreach (var chaPrecondition in editTarget.characterPreconditions) {
            EditorGUILayout.BeginHorizontal();
            charPrecontionWidgets[chaPrecondition].RenderUI();
            if (GUILayout.Button("X")) {
                toDelete.Add(chaPrecondition); 
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        // 删除被选中的
        foreach (var condition in toDelete) {
            editTarget.characterPreconditions.Remove(condition);
            charPrecontionWidgets.Remove(condition);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void RenderEnvironmentPrecontionUI() {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("环境");
        envPrecontionWidgets.RenderUI();
        EditorGUILayout.EndHorizontal();
    }

    private void RenderEventPreconditionUI() {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("前置事件");

        EditorGUILayout.BeginVertical();
        AddNewEventPreconditon();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        ShowExistedEventPreconditon();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    // TBD： 重构，name和card的绑定，应交由archive来做
    private void AddNewEventPreconditon() {
        int BUTTON_WIDTH = 20;
        List<string> allEventCardsNames = new List<string>();
        foreach (var eveCard in DeckArchive.instance.eventCards) {
            allEventCardsNames.Add(eveCard.name);
        }
        EditorGUILayout.BeginHorizontal();
        eventPreconditionPopupIndex = EditorGUILayout.Popup(eventPreconditionPopupIndex, allEventCardsNames.ToArray());
        if (GUILayout.Button("+", GUILayout.Width(BUTTON_WIDTH))) {
            // index 有效 
            if (eventPreconditionPopupIndex < DeckArchive.instance.eventCards.Count){
                var targetEventCard = DeckArchive.instance.eventCards[eventPreconditionPopupIndex];
                // 保证未添加过
                if (!editTarget.eventPreconditions.Exists((n) => n.eventCardName == targetEventCard.name)) {
                    var newCondition = new EventPrecondition(targetEventCard);
                    editTarget.eventPreconditions.Add(newCondition);
                    eventPrectionsDict[newCondition] = targetEventCard;
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void ShowExistedEventPreconditon() {
        EditorGUILayout.BeginHorizontal();
        var toDelete = new List<EventPrecondition>();

        foreach (var condition in editTarget.eventPreconditions) {
            EditorGUILayout.ObjectField(eventPrectionsDict[condition].GetAvatarImage(), typeof(Texture2D));
            if (GUILayout.Button("X", GUILayout.Width(20))) {
                toDelete.Add(condition);
            }
        }
        EditorGUILayout.EndHorizontal();

        foreach(var d in toDelete){
            editTarget.eventPreconditions.Remove(d);
            eventPrectionsDict.Remove(d);
        }
    }
}

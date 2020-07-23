using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PreconditionEditWidget : Widget {

    private PreconditonSet editTarget;

    // environment preconditon
    private SelectAndAddWidget<Qualifier> addEnvQualifierWidget;
    private DynamicWidgetGroup<QualifierWidget, Qualifier> envQualiferWidgetGroup;

    private Dictionary<EventPrecondition, EventCard> eventPrectionsDict;
    private int eventPreconditionPopupIndex = 0;

    // character precondtion
    private DynamicWidgetGroup<CharacterPreconditonWidget, CharacterPrecondition> chaPreconditonWidgetGroup;

    public PreconditionEditWidget(PreconditonSet target) {
        editTarget = target;

        // init environment widgets
        List<Qualifier> envQualifers = editTarget.environmentPrecondition.qualifiers;
        addEnvQualifierWidget = new SelectAndAddWidget<Qualifier>(
            envQualifers,
            () => DeckArchive.instance.environmentQualifierLib.GetQualiferNamesWithBlackList(envQualifers),
            (string name) =>new Qualifier(name));
        envQualiferWidgetGroup = new DynamicWidgetGroup<QualifierWidget, Qualifier>(editTarget.environmentPrecondition.qualifiers);


        chaPreconditonWidgetGroup = new DynamicWidgetGroup<CharacterPreconditonWidget, CharacterPrecondition>(editTarget.characterPreconditions, false);


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

    public override void RenderUI() {
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
        }
        chaPreconditonWidgetGroup.RenderUI();

        EditorGUILayout.EndHorizontal();
    }

    private void RenderEnvironmentPrecontionUI() {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("环境");
        addEnvQualifierWidget.RenderUI();
        envQualiferWidgetGroup.RenderUI();
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

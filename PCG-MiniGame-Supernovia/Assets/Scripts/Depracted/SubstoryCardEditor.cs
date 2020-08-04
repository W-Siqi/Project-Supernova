using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PCG {
    public class SubstoryCardEditor : CardEditor {
        private DynamicWidgetGroup<CardReferenceWidget, CharacterCard> chacaterCardWidgetGroup;
        private DynamicWidgetGroup<CardReferenceWidget, StratagemCard> staragemCardWidgetGroup;
        private DynamicWidgetGroup<CardReferenceWidget, EventCard> eventCardWidgetGroup;

        protected override void Init(Card editTarget) {
            base.Init(editTarget);
            var substoryCard = editTarget as SubstoryCard;
            chacaterCardWidgetGroup = new DynamicWidgetGroup<CardReferenceWidget, CharacterCard>(substoryCard.newCharacters);
            staragemCardWidgetGroup = new DynamicWidgetGroup<CardReferenceWidget, StratagemCard>(substoryCard.stratagemCards);
            eventCardWidgetGroup = new DynamicWidgetGroup<CardReferenceWidget, EventCard>(substoryCard.eventCards);
        }

        protected override void OnGUI() {
            base.OnGUI();
            int buttonWidth = 100;
            var substoryCard = editTarget as SubstoryCard;

            substoryCard.type = (SubstoryCard.Type)EditorGUILayout.EnumPopup(substoryCard.type);

            // chacarater
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+人物", GUILayout.Width(buttonWidth))) {
                var newCharacterInSubsotry = new CharacterCard();
                substoryCard.newCharacters.Add(newCharacterInSubsotry);
            }
            chacaterCardWidgetGroup.RenderUI();
            EditorGUILayout.EndHorizontal();

            // stratagem
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+决策", GUILayout.Width(buttonWidth))) {
                var newStratagem = new StratagemCard();
                substoryCard.stratagemCards.Add(newStratagem);
            }
            staragemCardWidgetGroup.RenderUI();
            EditorGUILayout.EndHorizontal();

            // chacarater
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+事件", GUILayout.Width(buttonWidth))) {
                var newEvent = new EventCard();
                substoryCard.eventCards.Add(newEvent);
            }
            eventCardWidgetGroup.RenderUI();
            EditorGUILayout.EndHorizontal();
        }
    }
}
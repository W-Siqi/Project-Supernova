using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PCG {
    public class EventCardEditor : CardEditor {
        private DescriptionBlock descriptionBlock;

        protected override void OnGUI() {
            base.OnGUI();
            var eventCard = editTarget as EventCard;
            eventCard.isAanonymous = EditorGUILayout.Toggle("匿名", eventCard.isAanonymous);
            descriptionBlock.RenderUI();
        }

        protected override void Init(Card editTarget) {
            base.Init(editTarget);
            descriptionBlock = new DescriptionBlock(editTarget as EventCard);
        }
    }
}

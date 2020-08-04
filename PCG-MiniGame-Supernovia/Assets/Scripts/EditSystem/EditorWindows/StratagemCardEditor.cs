using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PCG {
    public class StratagemCardEditor : CardEditor {
        private DescriptionBlock descriptionBlock;

        protected override void Init(Card editTarget) {
            base.Init(editTarget);
            descriptionBlock = new DescriptionBlock(editTarget as StratagemCard);
        }

        protected override void OnGUI() {
            base.OnGUI();
            var stratagemCard = editTarget as StratagemCard;
            stratagemCard.yesText = EditorGUILayout.TextField("yes text", stratagemCard.yesText);
            stratagemCard.noText = EditorGUILayout.TextField("no text", stratagemCard.noText);
            descriptionBlock.RenderUI();
        }
    }

}
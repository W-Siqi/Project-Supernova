using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PCG {
    public class StratagemCardEditor : CardEditor {
        protected override void Init(Card editTarget) {
            base.Init(editTarget);
        }

        protected override void OnGUI() {
            base.OnGUI();
            var stratagemCard = editTarget as StratagemCard;
            stratagemCard.yesText = EditorGUILayout.TextField("yes text", stratagemCard.yesText);
            stratagemCard.noText = EditorGUILayout.TextField("no text", stratagemCard.noText);
        }
    }

}
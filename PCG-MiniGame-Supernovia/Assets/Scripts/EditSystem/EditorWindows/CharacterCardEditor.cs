using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PCG;

namespace PCG {
    public class CharacterCardEditor : CardEditor {
        protected override void OnGUI() {
            base.OnGUI();
            var characterCard = editTarget as CharacterCard;
            CharacterAttribueEdit(characterCard);
        }

        protected override void Init(Card editTarget) {
            base.Init(editTarget);
            var targetCharacterCard = (CharacterCard)editTarget;
        }

        // 编辑属性
        private void CharacterAttribueEdit(CharacterCard characterCard) {
            characterCard.loyalty = EditorGUILayout.IntSlider("忠诚", characterCard.loyalty, 0, 10, GUILayout.Width(WINDOW_WIDTH));
            //characterCard.attributes.atkVal = EditorGUILayout.IntSlider("攻击力",characterCard.attributes.atkVal,0,10,GUILayout.Width(WINDOW_WIDTH));
            //characterCard.attributes.maxHP= EditorGUILayout.IntSlider("最大HP",characterCard.attributes.maxHP,1,20,GUILayout.Width(WINDOW_WIDTH));
            //characterCard.attributes.HP = characterCard.attributes.maxHP;
            //GUI.enabled = false;
            //EditorGUILayout.IntSlider("初始HP", characterCard.attributes.HP, 1, 20, GUILayout.Width(WINDOW_WIDTH));
            //GUI.enabled = true;
        }
    }
}

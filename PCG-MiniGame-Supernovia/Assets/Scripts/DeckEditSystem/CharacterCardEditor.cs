using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterCardEditor : CardEditor
{
    protected override void OnGUI() {
        base.OnGUI();
        var characterCard = editTarget as CharacterCard;
        CharacterAttribueEdit(characterCard);
    }
  

    // 编辑属性
    private void CharacterAttribueEdit(CharacterCard characterCard) {
        characterCard.attributes.competent = EditorGUILayout.Slider("能力",characterCard.attributes.competent,0,10,GUILayout.Width(WINDOW_WIDTH));
        characterCard.attributes.loyal = EditorGUILayout.Slider("忠诚",characterCard.attributes.loyal,0,10,GUILayout.Width(WINDOW_WIDTH));
        characterCard.attributes.shrewdness = EditorGUILayout.Slider("城府",characterCard.attributes.shrewdness,0,10,GUILayout.Width(WINDOW_WIDTH));
    }
}

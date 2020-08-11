//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityEditor.UIElements;
//using PCG;

//public class ShowInfoWidget : Widget{
//    private ShowInfo editTarget;
//    private SearchCardBlock searchCardBlock;
//    public ShowInfoWidget(ShowInfo editTarget) {
//        this.editTarget = editTarget;
//        searchCardBlock = new SearchCardBlock( 
//            SearchCardBlock.SearchTarget.characterCard,
//            (selected)=> { this.editTarget.target = selected as CharacterCard; });
//    }

//    public override void RenderUI() {
//        EditorGUILayout.BeginVertical();
//        searchCardBlock.RenderUI();
//        var cardRef = new CardReferenceWidget(editTarget.target);
//        cardRef.RenderUI();
//        editTarget.type =(ShowInfo.Type) EditorGUILayout.EnumPopup(editTarget.type);
//        EditorGUILayout.EndHorizontal();
//    }
//}

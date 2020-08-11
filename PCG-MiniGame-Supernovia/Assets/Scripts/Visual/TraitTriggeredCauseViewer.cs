using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PCG {
    public class TraitTriggeredCauseViewer : MonoBehaviour {
        [SerializeField]
        AudioSource causeAudio;
        [SerializeField]
        PositionTween charcracterShowup;
        [SerializeField]
        RawImage characterImage;

        [SerializeField]
        PositionTween traitShowup;
        [SerializeField]
        PersonalityViewerUGUI personalityViewer;

        [SerializeField]
        PositionTween tittleShowup;

        [SerializeField]
        PositionTween tooltipShowup;
        [SerializeField]
        Text tooptipText;

        // 显示触发性格的起因
        public void ViewCause(GameState gameState, GameStateModifyCause cause, bool samePreviousCharacter = false) {
            causeAudio.Play();

            var belongedCharacter = gameState.characterDeck[cause.belongedCharacterIndex];
            characterImage.texture = belongedCharacter.GetAvatarImage();

            personalityViewer.InitTo(cause.trait);
            ViewManager.instance.characterStausPannel.HightlightTrait(belongedCharacter, cause.trait);
            ViewManager.instance.characterStausPannel.ViewSentance(belongedCharacter, TraitUtils.GetTraitSlogan(cause.trait));
            tooptipText.text = TraitUtils.GetTooltip(cause.trait);

     
            charcracterShowup.Play();
            tittleShowup.Play();
            traitShowup.Play();
            tooltipShowup.Play();
        }

        public void EndViewCause() {
            charcracterShowup.Play(true);
            traitShowup.Play(true);
            tittleShowup.Play(true);
            tooltipShowup.Play(true);
        }
    }
}
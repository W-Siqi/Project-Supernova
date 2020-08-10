using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PCG {
    public class TraitTriggeredCauseViewer : MonoBehaviour {
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
        TextMeshProUGUI tooptipText;

        // 显示触发性格的起因
        public void ViewCause(GameStateModifyCause cause, bool samePreviousCharacter = false) {
            characterImage.texture = cause.belongedCharacter.GetAvatarImage();

            personalityViewer.InitTo(cause.trait);
            ViewManager.instance.characterStausPannel.HightlightTrait(cause.belongedCharacter, cause.trait);
            ViewManager.instance.characterStausPannel.ViewSentance(cause.belongedCharacter, TraitUtils.GetTraitSlogan(cause.trait));
            tooptipText.text = TraitUtils.GetTooltip(cause.trait);

            if (!samePreviousCharacter) {
                charcracterShowup.Play();
                tittleShowup.Play();
            }
            else {
                charcracterShowup.SetToEnd();
                tittleShowup.SetToEnd();
            }
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
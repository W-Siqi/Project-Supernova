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
        public void ViewCause(GameStateModifyCause cause) {

            characterImage.texture = cause.belongedCharacter.GetAvatarImage();
            personalityViewer.InitTo(cause.trait);
            tooptipText.text = TraitUtils.GetTooltip(cause.trait);
            charcracterShowup.Play();
            traitShowup.Play();
            tittleShowup.Play();
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
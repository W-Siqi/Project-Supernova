using PCG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TraitOverlapSign : MonoBehaviour
{
    
    [SerializeField]
    TextMeshProUGUI signText;
    [SerializeField]
    ImageGUIColorTween bgColorShowAndHide;
    [SerializeField]
    TextColorTween textColorShowAndHide;
    [SerializeField]
    SizeTween sizeShowUp;
    [SerializeField]
    Color evilColor;
    [SerializeField]
    Color noneEvilColor;
    public void ShowSign(Trait trait) {
        if (TraitUtils.IsEvil(trait)) {
            bgColorShowAndHide.endColor = evilColor;
        }
        else {
            bgColorShowAndHide.endColor = noneEvilColor;
        }
        signText.text = TraitUtils.TranslateToName(trait);
        sizeShowUp.Play();
        bgColorShowAndHide.Play();
        textColorShowAndHide.Play();
    }

    [ContextMenu("TEST")]
    private void Test() {
        ShowSign(Trait.corrupt);
    }
}

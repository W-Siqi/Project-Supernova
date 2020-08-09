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
    TextColorTween signShine;
    public void ShowSign(Trait trait) {
        if (TraitUtils.IsEvil(trait)) {
            signShine.endColor = ViewManager.instance.resTable.evilTraitColor;
        }
        else {
            signShine.endColor = ViewManager.instance.resTable.noneEvilTraitColor;
        }
        signText.text = TraitUtils.TranslateToName(trait);
        signShine.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;
using TMPro;
using UnityEngine.UI;

public class PersonalityViewerUGUI : MonoBehaviour
{
    private const string DISSOLVE_SHADER_PROPERTY = "_SliceAmount";
    public Trait currentViewedTrait { get; private set; } = Trait.none;
    [SerializeField]
    SizeTween hightlightSizeTween;
    [SerializeField]
    Image background;
    [SerializeField]
    TextMeshProUGUI traitName;

    public void InitTo(Trait trait) {
        currentViewedTrait = trait;
        traitName.text = TraitUtils.TranslateToName(trait);
        background.color = GetTraitColor(trait);
    }

    public void TransferTo(Trait trait) {
        currentViewedTrait = trait;
        traitName.text = TraitUtils.TranslateToName(trait);
        background.color = GetTraitColor(trait);
        Highlight();
    }

    public void Highlight() {
        hightlightSizeTween.Play();
    }

    private Color GetTraitColor(Trait trait) {
        if (TraitUtils.IsEvil(trait)) {
            return ViewManager.instance.resTable.evilTraitColor;
        }
        else {
            return ViewManager.instance.resTable.noneEvilTraitColor;
        }
    }
}

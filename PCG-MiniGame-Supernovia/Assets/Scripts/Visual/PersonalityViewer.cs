using PCG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class PersonalityViewer : MonoBehaviour
{
    private const string DISSOLVE_SHADER_PROPERTY = "_SliceAmount";
    public Trait currentViewedTrait { get; private set; } = Trait.none;

    [SerializeField]
    SpriteRenderer background;
    [SerializeField]
    RuntimeMaterial backgroundMat;
    [SerializeField]
    TextMeshPro traitName;

    public void InitTo(Trait trait) {
        currentViewedTrait = trait;
        traitName.text = TraitUtils.TranslateToName(trait);
        backgroundMat.Init();
        background.sprite = GetTraitBackground(trait);
    }

    public void TransferTo(Trait trait) {
        currentViewedTrait = trait;
        StartCoroutine(TransferAnimation(trait));
    }

    private Sprite GetTraitBackground(Trait trait) {
        if (TraitUtils.IsEvil(trait)) {
            return ResourceTable.instance.texturepage.evilTraitBG;
        }
        else {
            return ResourceTable.instance.texturepage.noneEvilTraitBG;
        }  
    }

    private IEnumerator TransferAnimation(Trait trait) {
        float dissolveDuration = 2f;
        traitName.text = "";
        LerpAnimator.instance.LerpValues(0, 1, dissolveDuration,
            (v) => {
                backgroundMat.runtimeMat.SetFloat(DISSOLVE_SHADER_PROPERTY, v);
            });

        yield return new WaitForSeconds(dissolveDuration);

        traitName.text = TraitUtils.TranslateToName(trait);
        background.sprite = GetTraitBackground(trait);
        LerpAnimator.instance.LerpValues(1, 0, dissolveDuration,
            (v) => {
                backgroundMat.runtimeMat.SetFloat(DISSOLVE_SHADER_PROPERTY, v);
            });

        yield return new WaitForSeconds(dissolveDuration);
    }
}

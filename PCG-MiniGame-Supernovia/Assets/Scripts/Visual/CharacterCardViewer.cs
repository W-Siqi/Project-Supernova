using PCG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardViewer : MonoBehaviour
{
    [SerializeField]
    private PositionTween spawnPositonTween;
    [SerializeField]
    private List<PersonalityViewerUGUI> personalityViewerUGUIs = new List<PersonalityViewerUGUI>();
    [SerializeField]
    private Image avatarImage;
    [SerializeField]
    private TraitOverlapSign traitAddSign;
    [SerializeField]
    SizeTween santanceSizeTween;
    [SerializeField]
    TextMeshProUGUI santanceText;
    [SerializeField]
    Image removeTraitImage;
    private CharacterCard hookedCharacter = null;

    public static CharacterCardViewer Create(CharacterCard character, Transform spawnAnchor, Transform targetAnchor) {
        var GO = Instantiate(ResourceTable.instance.prefabPage.characterCardViewer);
        var viewer = GO.GetComponent<CharacterCardViewer>();
        viewer.transform.SetParent(spawnAnchor.parent);
        viewer.HookTo(character);
        viewer.spawnPositonTween.startAnchor = spawnAnchor;
        viewer.spawnPositonTween.endAnchor = targetAnchor;
        viewer.spawnPositonTween.Play();
        return viewer;
    }

    public void HookTo(CharacterCard character) {
        hookedCharacter = character;
        avatarImage.sprite = hookedCharacter.GetAvatarSprite();

        // personality
        if (hookedCharacter.personalities.Length != personalityViewerUGUIs.Count) {
            throw new System.Exception("UI个数不对");
        }
        for (int i = 0; i < hookedCharacter.personalities.Length; i++) {
            personalityViewerUGUIs[i].InitTo(hookedCharacter.personalities[i].trait);
        }
    }

    public void HighlightTrait(Trait trait) {
        // personality viewer自带的hight
        foreach (var personalityViewer in personalityViewerUGUIs) {
            if (personalityViewer.currentViewedTrait == trait) {
                personalityViewer.HighlightOn();
            }
        }
        // 放一个符合特质的santance 
        DoSantence(TraitUtils.GetTraitSlogan(trait));
    }

    public void OnTraitRemove(Trait trait) {
        // personality 对应打个红叉
        foreach (var personalityViewer in personalityViewerUGUIs) {
            if (personalityViewer.currentViewedTrait == trait) {
                removeTraitImage.transform.position = personalityViewer.transform.position;
                break;
            }
        }
        // 放一个符合特质的santance 
        DoSantence(string.Format("我再" + TraitUtils.GetFullName(trait)+"又有何用"));
    }

    public void OnTraitAdd(Trait trait) {
        //trait sign
        traitAddSign.ShowSign(trait);
        // 放一个符合特质的santance 
        DoSantence(string.Format("我竟会变得"+TraitUtils.GetFullName(trait)));
    }

    private void DoSantence(string content) {
        santanceSizeTween.gameObject.SetActive(true);
        santanceSizeTween.Play();
        santanceText.text = content;
    }
}
